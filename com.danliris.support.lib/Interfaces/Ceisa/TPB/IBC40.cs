﻿using com.danliris.support.lib.Helpers;
using com.danliris.support.lib.Models.Ceisa.TPB;
using com.danliris.support.lib.ViewModel.Ceisa.TPBViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.danliris.support.lib.Interfaces.Ceisa
{
    public interface IBC40
    {
        ReadResponse<object> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
        string Urut();
        Task<int> Create(TPBHeader model);
        Task<TPBHeader> ReadById(long id);
        Task<int> UpdateAsync(int id, TPBHeader viewModel);
        Task<int> Delete(int id);
        Task<int> PostToSupportTPB(int id, TPBViewModelList viewModel);
    }
}
