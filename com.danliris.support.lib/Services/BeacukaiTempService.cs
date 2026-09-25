using com.danliris.support.lib.Models;
using com.danliris.support.lib.ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.danliris.support.lib.Services
{
    public class BeacukaiTempService : IBeacukaiTempService
    {
        private readonly SupportDbContext context;

        public BeacukaiTempService(SupportDbContext context)
        {
            this.context = context;
        }

        public List<BEACUKAI_TEMPViewModel> Get(int size = 25, string keyword = null)
        {
            string[] bcType = { "BC 262", "BC 23", "BC 40", "BC 27" };

            IQueryable<BEACUKAI_TEMP> Query = context.BeacukaiTemp.Where(s => bcType.Contains(s.JenisBC) && s.TglBCNo.Year >= DateTime.Now.Year - 1 && s.Barang != null);


            var Query2 = Query
                .Select(p => new BEACUKAI_TEMPViewModel
                {

                    BCNo = p.BCNo,
                    BCId = p.BCId, //BonNo = p.BonNo,
                    TglBCNo = p.TglBCNo, //BCDate = p.BCDate,
                    JenisBC = p.JenisBC, //BCType = p.BCType,
                    TglDatang = p.TglDatang,
                    Hari = p.Hari,
                    Netto = Convert.ToDouble(p.Netto),
                    Bruto = Convert.ToDouble(p.Bruto),
                    KodeKemasan = p.KodeKemasan,
                    JumlahKemasan = p.JumlahKemasan != null ? p.JumlahKemasan : 0,
                });

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                Query2 = Query2.Where(s => s.BCNo.StartsWith(keyword));
            }

            Query2 = Query2
                .OrderBy(o => o.BCNo)
                .Distinct();

            var Query3 = Query2.GroupBy(x => new { x.BCNo, x.JenisBC, x.TglBCNo }, (key, group) => new BEACUKAI_TEMPViewModel
            {
                BCNo = key.BCNo,
                BCId = group.FirstOrDefault().BCId, //BonNo = p.BonNo,
                TglBCNo = key.TglBCNo, //BCDate = p.BCDate,
                JenisBC = key.JenisBC, //BCType = p.BCType,
                TglDatang = group.FirstOrDefault().TglDatang,
                Hari = group.FirstOrDefault().Hari,
                Netto = Convert.ToDouble(group.FirstOrDefault().Netto),
                Bruto = Convert.ToDouble(group.FirstOrDefault().Bruto),
                JumlahKemasan = group.FirstOrDefault().JumlahKemasan,
                KodeKemasan = group.FirstOrDefault().KodeKemasan

            });

            Query3 = Query3
                .Take(size);

            return Query3.ToList();
        }

        public async Task<List<BEACUKAI_ToDeleteViewModel>> GetDataForDelete(string jenis, string type,string nomor)
        {
            List<BEACUKAI_ToDeleteViewModel> data = new List<BEACUKAI_ToDeleteViewModel>();
            if(jenis == "TPB")
            {
                var Query = type == "Nomor Aju" ?
                    await context.BeacukaiTemp.Where(x => x.NoAju.Contains(nomor) && (x.TglBCNo.Month >= DateTime.Now.Month - 6 && x.TglBCNo.Year >= DateTime.Now.Year - 1))
                    .Select(a => new BEACUKAI_ToDeleteViewModel 
                    {
                        NoAju = a.NoAju,
                        TglBCNo = a.TglBCNo,
                        BCNo = a.BCNo,
                        JenisBC = a.JenisBC,
                        CreatedBy = a.CreatedBy
                    })
                    .ToListAsync() :
                    await context.BeacukaiTemp.Where(x => x.BCNo.Contains(nomor) && (x.TglBCNo.Month >= DateTime.Now.Month - 6 && x.TglBCNo.Year >= DateTime.Now.Year - 1))
                     .Select(a => new BEACUKAI_ToDeleteViewModel
                     {
                         NoAju = a.NoAju,
                         TglBCNo = a.TglBCNo,
                         BCNo = a.BCNo,
                         JenisBC = a.JenisBC,
                         CreatedBy = a.CreatedBy
                     }).ToListAsync();

                var QueryGroup = Query.GroupBy(x => new { x.NoAju, x.TglBCNo, x.BCNo, x.JenisBC,x.CreatedBy }, (key, group) => new BEACUKAI_ToDeleteViewModel
                {
                    NoAju = key.NoAju,
                    TglBCNo = key.TglBCNo,
                    BCNo = key.BCNo,
                    JenisBC = key.JenisBC,
                    CreatedBy = key.CreatedBy
                });

                foreach (var a in QueryGroup)
                {
                    data.Add(a);
                }
            }
            else if(jenis == "PEB")
            {
                var Query = type == "Nomor Aju" ?
                   await context.BEACUKAI_ADDED.Where(x => x.CAR.Contains(nomor) && (x.BCDate.Month >= DateTime.Now.Month - 6 && x.BCDate.Year >= DateTime.Now.Year - 1))
                   .Select(a => new BEACUKAI_ToDeleteViewModel
                   {
                       NoAju = a.CAR,
                       TglBCNo = a.BCDate,
                       BCNo = a.BCNo,
                       JenisBC = a.BCType,
                       CreatedBy = a.CreateUser
                   })
                   .ToListAsync() :
                   await context.BEACUKAI_ADDED.Where(x => x.BCNo.Contains(nomor) && (x.BCDate.Month >= DateTime.Now.Month - 6 && x.BCDate.Year >= DateTime.Now.Year - 1))
                    .Select(a => new BEACUKAI_ToDeleteViewModel
                    {
                        NoAju = a.CAR,
                        TglBCNo = a.BCDate,
                        BCNo = a.BCNo,
                        JenisBC = a.BCType,
                        CreatedBy = a.CreateUser
                    }).ToListAsync();

                var QueryGroup = Query.GroupBy(x => new { x.NoAju, x.TglBCNo, x.BCNo, x.JenisBC, x.CreatedBy }, (key, group) => new BEACUKAI_ToDeleteViewModel
                {
                    NoAju = key.NoAju,
                    TglBCNo = key.TglBCNo,
                    BCNo = key.BCNo,
                    JenisBC = key.JenisBC,
                    CreatedBy = key.CreatedBy
                });


                foreach (var a in QueryGroup)
                {
                    data.Add(a);
                }
            }

            return data;
        }

        public async Task<int> DeleteData(List<BEACUKAI_ToDeleteViewModel> AjuToDelete)
        {
            int Delete = 0;

            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    foreach(var data in AjuToDelete)
                    {
                        if(data.Jenis == "TPB")
                        {
                            var header = context.BeacukaiTemp.Where(x => x.NoAju == data.NoAju);

                            context.BeacukaiTemp.RemoveRange(header);
                        }else if (data.Jenis == "PEB")
                        {
                            var header = context.BEACUKAI_ADDED.Where(x => x.CAR == data.NoAju);

                            var detail = context.BEACUKAI_ADDED_DETAIL.Where(x => x.CAR == data.NoAju);

                            context.BEACUKAI_ADDED.RemoveRange(header);
                            context.BEACUKAI_ADDED_DETAIL.RemoveRange(detail);
                        }
                        
                    }
                    Delete = await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

            return Delete;
        }
        public MemoryStream GenerateExcelReport(string type, string dateFrom, string dateTo, string timezoneOffset)
        {
            int offsetHour = 0;

            if (!string.IsNullOrWhiteSpace(timezoneOffset))
            {
                int.TryParse(timezoneOffset, out offsetHour);
            }

            var offset = TimeSpan.FromHours(offsetHour);

            var localDateFrom = DateTime.Parse(dateFrom).Date;
            var localDateTo = DateTime.Parse(dateTo).Date.AddDays(1);

            // Konversi range tanggal lokal ke UTC
            var startDate = new DateTimeOffset(localDateFrom, offset).UtcDateTime;
            var endDate = new DateTimeOffset(localDateTo, offset).UtcDateTime;

            var data = context.BeacukaiTemp
                .AsNoTracking()
                .Where(x =>
                    (string.IsNullOrEmpty(type) || x.JenisBC == type) &&
                    x.TglBCNo >= startDate &&
                    x.TglBCNo < endDate)
                .Select(x => new
                {
                    x.BCId,
                    x.BCNo,
                    x.JenisBC,
                    x.TglBCNo,
                    x.JenisDokumen,
                    x.NomorDokumen,
                    x.TanggalDokumen
                })
                .ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Report Bea Cukai");

                // Header
                worksheet.Cells[1, 1].Value = "Beacukai Id";
                worksheet.Cells[1, 2].Value = "Nomor Beacukai";
                worksheet.Cells[1, 3].Value = "Jenis BC";
                worksheet.Cells[1, 4].Value = "Beacukai Date";
                worksheet.Cells[1, 5].Value = "Jenis Dokumen";
                worksheet.Cells[1, 6].Value = "Nomor Dokumen";
                worksheet.Cells[1, 7].Value = "Tanggal Dokumen";

                var row = 2;

                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.BCId;
                    worksheet.Cells[row, 2].Value = item.BCNo;
                    worksheet.Cells[row, 3].Value = item.JenisBC;

                    worksheet.Cells[row, 4].Value = item.TglBCNo.AddHours(offsetHour);
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "dd/MM/yyyy";

                    worksheet.Cells[row, 5].Value = item.JenisDokumen;
                    worksheet.Cells[row, 6].Value = item.NomorDokumen;

                    if (item.TanggalDokumen.HasValue)
                    {
                        worksheet.Cells[row, 7].Value = item.TanggalDokumen.Value.AddHours(offsetHour);

                        worksheet.Cells[row, 7].Style.Numberformat.Format = "dd/MM/yyyy";
                    }

                    row++;
                }

                // Header style
                using (var header = worksheet.Cells[1, 1, 1, 7])
                {
                    header.Style.Font.Bold = true;
                    header.Style.Font.Color.SetColor(System.Drawing.Color.White);

                    header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    header.Style.Fill.BackgroundColor.SetColor(
                        System.Drawing.Color.FromArgb(68, 114, 196)
                    );

                    header.Style.HorizontalAlignment =
                        ExcelHorizontalAlignment.Center;

                    header.Style.VerticalAlignment =
                        ExcelVerticalAlignment.Center;
                }

                // Zebra row
                for (var i = 2; i < row; i++)
                {
                    if (i % 2 == 0)
                    {
                        using (var rowRange = worksheet.Cells[i, 1, i, 7])
                        {
                            rowRange.Style.Fill.PatternType =
                                ExcelFillStyle.Solid;

                            rowRange.Style.Fill.BackgroundColor.SetColor(
                                System.Drawing.Color.FromArgb(221, 235, 247)
                            );
                        }
                    }
                }

                // Border
                if (row > 2)
                {
                    using (var range = worksheet.Cells[1, 1, row - 1, 7])
                    {
                        range.Style.Border.Top.Style =
                            ExcelBorderStyle.Thin;

                        range.Style.Border.Bottom.Style =
                            ExcelBorderStyle.Thin;

                        range.Style.Border.Left.Style =
                            ExcelBorderStyle.Thin;

                        range.Style.Border.Right.Style =
                            ExcelBorderStyle.Thin;
                    }
                }

                // Auto Filter
                if (row > 2)
                {
                    worksheet.Cells[1, 1, row - 1, 7]
                        .AutoFilter = true;
                }

                // Freeze Header
                worksheet.View.FreezePanes(2, 1);

                // Auto Fit
                worksheet.Cells[worksheet.Dimension.Address]
                    .AutoFitColumns();

                package.Save();
            }

            stream.Position = 0;

            return stream;
        }
    }

   

    public interface IBeacukaiTempService
    {
        List<BEACUKAI_TEMPViewModel> Get(int size = 25, string keyword = null);
        Task<List<BEACUKAI_ToDeleteViewModel>> GetDataForDelete(string jenis, string type, string nomor);
        Task<int> DeleteData(List<BEACUKAI_ToDeleteViewModel> AjuToDelete);
        MemoryStream GenerateExcelReport(string type, string dateFrom, string dateTo, string timezoneOffset);
    }
}
