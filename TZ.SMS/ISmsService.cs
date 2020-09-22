using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TZ.SMS
{
    public interface ISmsService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号，多个用逗号隔开</param>
        /// <param name="signName">签名名字</param>
        /// <param name="templateCode">短信模板Id</param>
        /// <param name="outId">外部流水扩展字段</param>
        /// <param name="templateParamDic">自定义参数</param>
        /// <returns></returns>
        (bool success, string msg) SendSmsTemplate(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null);


        /// <summary>
        /// 异步发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号，多个用逗号隔开</param>
        /// <param name="signName">签名名字</param>
        /// <param name="templateCode">短信模板Id</param>
        /// <param name="outId">外部流水扩展字段</param>
        /// <param name="templateParamDic">自定义参数</param>
        /// <returns></returns>
        Task<(bool success, string msg)> SendSmsTemplateAsync(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null);
    }
}
