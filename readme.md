# TZ.SMS

[![nuget](https://img.shields.io/nuget/v/TZ.SMS.svg?style=flat-square)](https://www.nuget.org/packages/TZ.SMS) 
[![stats](https://img.shields.io/nuget/dt/TZ.SMS.svg?style=flat-square)](https://www.nuget.org/stats/packages/TZ.SMS?groupby=Version)
[![License](https://img.shields.io/badge/license-Apache2.0-blue.svg)](https://github.com/tanyongzheng/TZ.SMS/blob/master/LICENSE)
![.NETStandard](https://img.shields.io/badge/.NETStandard-%3E%3D2.0-green.svg)

## 介绍
发送短信类库

主要对接第三方短信：
1. 阿里云短信


## 使用说明

1. Install-Package TZ.SMS

2. 注入服务：
```cs
    services.AddAliyunSms(Configuration);
```

3. 阿里云短信配置
```js
    "AliyunSms": {
    "RegionId": "shenzheng",
    "AccessKeyId": "",
    "AccessSecret": ""
  }
```

4. 使用见项目Demo
