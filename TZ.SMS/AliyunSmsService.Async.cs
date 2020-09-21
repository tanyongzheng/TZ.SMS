using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TZ.SMS
{
    public partial class AliyunSmsService
    {
        /// <summary>
        /// 发送短信模板
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="signName">短信签名名称</param>
        /// <param name="templateCode">短信模板</param>
        /// <param name="outId">外部流水扩展字段</param>
        /// <param name="templateParamDic">模板中自定义参数及参数值</param>
        /// <returns></returns>
        public Task<(bool success, string msg)> SendSmsTemplateAsync(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null)
        {
            (bool success, string msg) result =
                SendSmsTemplate(phoneNumber, signName, templateCode, outId, templateParamDic);
            //阿里云SDK暂时没异步方法，等更新有异步方法后再重写
            //issue ：https://github.com/aliyun/aliyun-openapi-net-sdk/issues/163
            return Task.FromResult(result); ;
        }
    }
}
