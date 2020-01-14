using System;
using System.Collections.Generic;
using System.Text;

namespace TZ.SMS
{
    public interface ISmsService
    {
        (bool success, string msg) SendSmsTemplate(string phoneNumber, string signName, string templateCode, string outId = null, Dictionary<string, string> templateParamDic = null);
    }
}
