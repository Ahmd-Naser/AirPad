using AirPad.Hubs;
using AirPad.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using QRCoder;
using System.Net;
using System.Net.Sockets;

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

builder.Services.AddSingleton<IMouseSimulator, MockMouseSimulator>();

var app = builder.Build();

// 1. تفعيل الملفات الافتراضية (هذا سيجعل الرابط يعمل بدون كتابة index.html)
app.UseDefaultFiles();
// 2. تفعيل الملفات الثابتة
app.UseStaticFiles();

app.MapHub<AirPadHub>("/airpadHub");

await app.StartAsync();

// استخراج المنفذ والـ IP وطباعة الرابط الصحيح
var server = app.Services.GetService<IServer>();
var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses;
var port = addresses?.FirstOrDefault()?.Split(':').Last(); // استخراج المنفذ فقط

string localIp = GetLocalIPv4();
string directUrl = $"http://{localIp}:{port}";

Console.WriteLine("==================================================");
Console.WriteLine("🚀 AirPad Server is running!");
Console.WriteLine($"[Direct Link]: {directUrl}");
Console.WriteLine("==================================================");

using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
{
    // إنشاء بيانات الكود (بمستوى تصحيح خطأ منخفض L لتقليل حجم الكود لتسهيل قراءته من الشاشة)
    QRCodeData qrCodeData = qrGenerator.CreateQrCode(directUrl, QRCodeGenerator.ECCLevel.L);

    // 2. تحويله إلى نص (ASCII)
    AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);

    // 3. رسم الكود: نستخدم "██" للمربع الأسود، و "  " (مسافتين) للمربع الأبيض
    string qrCodeText = qrCode.GetGraphic(1, "██", "  ");

    // 4. طباعته في الكونسول
    Console.WriteLine(qrCodeText);
}


await app.WaitForShutdownAsync();


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