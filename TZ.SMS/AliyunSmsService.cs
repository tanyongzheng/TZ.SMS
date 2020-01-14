using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using System.Threading.Tasks;
using Aliyun.Acs.Core.Http;
using System;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.Extensions.Options;

namespace TZ.SMS
{
    /// <summary>
    /// 阿里云短信服务
    /// 文档参考：https://api.aliyun.com/?spm=a2c4g.11186623.2.15.48a41b4avRN23c#/?product=Dysmsapi
    /// </summary>
    public class AliyunSmsService: ISmsService
    {
        private AliyunSmsOptions aliyunSmsOptionsValue;
        public AliyunSmsService(IOptions<AliyunSmsOptions> options)
        {
            if (options == null || options.Value == null)
            {
                throw new Exception("please set AliyunSmsOptions!");
            }
            else if (options.Value != null)
            {
                aliyunSmsOptionsValue = options.Value;
            }
        }

        public (bool success, string msg) SendSmsTemplate2(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null)
        {
            (bool success, string msg) result = new ValueTuple<bool, string>();
            IClientProfile profile = DefaultProfile.GetProfile(aliyunSmsOptionsValue.RegionId,
                aliyunSmsOptionsValue.AccessKeyId,
                aliyunSmsOptionsValue.AccessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";

            request.AddQueryParameters("PhoneNumbers", phoneNumber);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", templateCode);

            //短信模板变量对应的实际值，JSON格式。说明 如果JSON中需要带换行符，请参照标准的JSON协议处理。
            if (templateParamDic != null && templateParamDic.Count > 0)
            {
                var converter = new IsoDateTimeConverter();
                converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                var jsonTemplateParam = JsonConvert.SerializeObject(templateParamDic, Formatting.Indented, converter);
                request.AddQueryParameters("TemplateParam", jsonTemplateParam);
            }
            //可选:外部流水扩展字段。
            request.AddQueryParameters("OutId", outId);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                var respStr=System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.MissingMemberHandling = MissingMemberHandling.Ignore;
                var sendSmsResponse=JsonConvert.DeserializeObject<AliyunSmsRespModel>(respStr, setting);            

                if (!string.IsNullOrEmpty(sendSmsResponse.Code) && sendSmsResponse.Code.ToUpper() == "OK")
                {
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.msg = sendSmsResponse.Message;
                }
            }
            catch (ServerException e)
            {
                result.success = false;
                result.msg = e.Message;
            }
            catch (ClientException e)
            {
                result.success = false;
                result.msg = e.Message;
            }
            return result;
        }
        
        /// <summary>
        /// 发送短信模板
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="signName">短信签名名称</param>
        /// <param name="templateCode">短信模板</param>
        /// <param name="outId">外部流水扩展字段</param>
        /// <param name="templateParamDic">模板中自定义参数及参数值</param>
        /// <returns></returns>
        public (bool success, string msg) SendSmsTemplate(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null)
        {
            (bool success, string msg) result = new ValueTuple<bool, string>();
            IClientProfile profile = DefaultProfile.GetProfile(aliyunSmsOptionsValue.RegionId,
                aliyunSmsOptionsValue.AccessKeyId,
                aliyunSmsOptionsValue.AccessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            var request = new SendSmsRequest();
            request.Method = MethodType.POST;
            request.Version = "2017-05-25";
            //支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。上限为1000个手机号码。批量调用相对于单条调用及时性稍有延迟。
            request.PhoneNumbers = phoneNumber;
            //必填:短信签名名称。请在控制台签名管理页面签名名称一列查看。
            request.SignName = signName;
            //必填:短信模板-可在短信控制台中找到
            request.TemplateCode = templateCode;

            //短信模板变量对应的实际值，JSON格式。说明 如果JSON中需要带换行符，请参照标准的JSON协议处理。
            if (templateParamDic != null && templateParamDic.Count > 0)
            {
                var converter = new IsoDateTimeConverter();
                converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                var jsonTemplateParam = JsonConvert.SerializeObject(templateParamDic, Formatting.Indented, converter);
                request.TemplateParam = jsonTemplateParam;
            }
            //可选:外部流水扩展字段。
            request.OutId = outId;
            try
            {
                var sendSmsResponse = client.GetAcsResponse(request);
                if (!string.IsNullOrEmpty(sendSmsResponse.Code) && sendSmsResponse.Code.ToUpper() == "OK")
                {
                    result.success = true;
                }
                else
                {
                    result.success = false;
                    result.msg = sendSmsResponse.Message;
                }
            }
            catch (ServerException e)
            {
                result.success = false;
                result.msg = e.Message;
            }
            catch (ClientException e)
            {
                result.success = false;
                result.msg = e.Message;
            }
            return result;
        }
    }


    public class AliyunSmsRespModel
    {
        public string Message { get; set; }
        public string RequestId { get; set; }
        public string BizId { get; set; }
        public string Code { get; set; }
    }

}
