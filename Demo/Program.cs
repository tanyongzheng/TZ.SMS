using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TZ.SMS;

namespace Demo
{
    class Program
    {

        private static IServiceProvider ServiceProvider { get; set; }
        private static IContainer ApplicationContainer { get; set; }
        private static IServiceCollection ServiceCollections { get; set; }

        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            SetConfiguration();
            ServiceCollections = new ServiceCollection();
            ConfigureServices(ServiceCollections);
            var smsService = ServiceProvider.GetService<ISmsService>();
            var phone = "138xxxx,130xxxx";
            var signName = "签名名字";
            var templateCode = "SMS_xxxxxx";
            var outId = "业务id";
            var templateParamDic = new Dictionary<string, string>();
            templateParamDic.Add("UserId", "123456");
            templateParamDic.Add("vCode", "ABC123");
            var sendResult = smsService.SendSmsTemplate(phone, signName, templateCode, outId, templateParamDic);
            if (!string.IsNullOrEmpty(sendResult.msg))
                Console.WriteLine(sendResult.msg);
            Console.ReadKey();
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        static int GetRandomNum(int minValue, int maxValue)
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int result = ran.Next(minValue, maxValue);
            System.Threading.Thread.Sleep(1);
            return result;
        }

        static void SetConfiguration()
        {
            #region 配置文件

            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "appsettings.json";

            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);
            Configuration = configurationBuilder.Build();
            #endregion
        }
        static void ConfigureServices(IServiceCollection services)
        {
            #region AliyunSms
            services.AddAliyunSms(Configuration);
            #endregion

            #region 注入容器
            var containerBuilder = new ContainerBuilder();//实例化 AutoFac  容器
            containerBuilder.RegisterDynamicProxy();//注册AOP动态代理，目前使用AspectCore//模块化注入，默认注入模块
            containerBuilder.Populate(services);//管道寄居
            containerBuilder.RegisterModule<DefaultRegisterModule>();
            ApplicationContainer = containerBuilder.Build();//IUserService UserService 构造 
            #endregion

            ServiceProvider = new AutofacServiceProvider(ApplicationContainer);//将autofac反馈到管道中
        }
    }
}
