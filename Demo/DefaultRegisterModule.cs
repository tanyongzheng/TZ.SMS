using Autofac;
using TZ.SMS;

namespace Demo
{
    /// <summary>
    /// 模块化注入，默认注入类型
    /// </summary>
    public class DefaultRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AliyunSmsService>().As<ISmsService>().InstancePerDependency();
        }
    }
}
