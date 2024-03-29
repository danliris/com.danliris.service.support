﻿using AutoMapper;
using com.danliris.support.lib.Interfaces.Ceisa;
using com.danliris.support.lib.Models.Ceisa.TPB;
using com.danliris.support.lib.ViewModel.Ceisa.TPBViewModel;
using com.danliris.support.webapi.Helpers;
using Com.DanLiris.Service.support.lib.Services;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.danliris.support.webapi.Controllers.v1.Ceisa.TPBController
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/tpb/261")]
    [Authorize]
    public class BC261Controller : Controller
    {
        private static readonly string ApiVersion = "1.0";
        public readonly IServiceProvider serviceProvider;
        public readonly IMapper Mapper;
        private IBC261 bC261Service;
        private readonly IdentityService identityService;
        public BC261Controller(IBC261 b261service, IServiceProvider serviceProvider, IMapper mapper)
        {
            this.bC261Service = b261service;
            this.serviceProvider = serviceProvider;
            this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
            this.Mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get(int page, int size, string keyword, string filter, string Order = "{}")
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                var data = bC261Service.Read(page, size, Order, keyword, filter);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Data,
                    info = new { total = data.TotalData },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpGet("count")]
        public IActionResult GetUrut()
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                var data = bC261Service.Urut();

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data,
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TPBHeader model)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                await bC261Service.Create(model);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(Request.Path, "/", 0), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                var data = await bC261Service.ReadById(id);

                TPBHeaderViewModel result = Mapper.Map<TPBHeaderViewModel>(data);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = result,
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                }
                );
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TPBHeader viewModel)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                await bC261Service.UpdateAsync(id, viewModel);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    message = General.UPDATE_MESSAGE,
                    statusCode = General.UPDATED_STATUS_CODE
                });
            }

            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                   .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                await bC261Service.Delete(id);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
        [HttpPut("support-TPB/{id}")]
        public async Task<IActionResult> PostToSupport(int id, [FromBody] TPBViewModelList viewModel)
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                await bC261Service.PostToSupportTPB(id, viewModel);
                return Ok(new
                {
                    apiVersion = ApiVersion,
                    message = General.UPDATE_MESSAGE,
                    statusCode = General.UPDATED_STATUS_CODE
                });
            }

            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                   .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
