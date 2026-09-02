using AirPad.Hubs;
using AirPad;
using AirPad.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using QRCoder;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

// ... (باقي الاستدعاءات الموجودة لديك مثل Hubs و Services و System.Net) ...

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(0);
});

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<IMouseSimulator, WindowsSimulator>();

var app = builder.Build();

// 1. تفعيل الملفات الافتراضية (هذا سيجعل الرابط يعمل بدون كتابة index.html)
app.UseDefaultFiles();
// 2. تفعيل الملفات الثابتة
app.UseStaticFiles();

app.MapHub<AirPadHub>("/airpadHub");

// بدء تشغيل سيرفر الويب في الخلفية (بدون إيقاف الكود هنا)
await app.StartAsync();

// استخراج الرابط
var server = app.Services.GetService<IServer>();
var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses;
var port = addresses?.FirstOrDefault()?.Split(':').Last();
string localIp = GetLocalIPv4();
string directUrl = $"http://{localIp}:{port}";

// إعداد بيئة WinForms وتشغيلها
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

// تشغيل النافذة التي أنشأناها وتمرير الرابط لها
var mainForm = new MainForm(directUrl);
Application.Run(mainForm);

// عندما يقوم المستخدم بإغلاق التطبيق من الأيقونة بجوار الساعة، سيصل الكود هنا لإغلاق السيرفر بسلام
await app.StopAsync();

// (دالة GetLocalIPv4 تبقى كما هي في أسفل الملف)
// دالة مساعدة لاستخراج عنوان IPv4 المحلي الخاص بالواي فاي/الشبكة
static string GetLocalIPv4()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
        {
            return ip.ToString();
        }
    }
    return "127.0.0.1";
}