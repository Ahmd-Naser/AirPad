// Program.cs
using AirPad.Hubs;
using AirPad.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// 1. ضبط خادم Kestrel ليعمل على منفذ ديناميكي (Port 0)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // السماح بالاتصال من أي جهاز على الشبكة (Local Network)
    serverOptions.ListenAnyIP(0);
});

// 2. تسجيل الخدمات (Dependency Injection)
builder.Services.AddSignalR(options =>
{
    // ضبط فترات الاتصال لحل مشكلة التذبذب
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
});
// استخدام Singleton لضمان وجود نسخة واحدة فقط من المتحكم بالماوس
builder.Services.AddSingleton<IMouseSimulator, MockMouseSimulator>();

var app = builder.Build();

// 3. إعداد مسارات الـ Middleware
// تفعيل قراءة الملفات من مجلد wwwroot (للواجهة الأمامية)
app.UseStaticFiles();

// تحديد المسار الذي سيتصل عليه الموبايل
app.MapHub<AirPadHub>("/airpadHub");

// 4. تشغيل التطبيق (بدون إيقاف الكود هنا لنتمكن من قراءة المنفذ)
await app.StartAsync();

// 5. استخراج المنفذ الديناميكي الذي اختاره نظام التشغيل وطباعته
var server = app.Services.GetService<IServer>();
var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses;

Console.WriteLine("==================================================");
Console.WriteLine("🚀 AirPad Server is running!");
if (addresses != null)
{
    foreach (var address in addresses)
    {
        Console.WriteLine($"[Listening on]: {address}");
    }
    Console.WriteLine("⚠️ Note: Replace '[::]' or '0.0.0.0' with your actual Local IPv4 (e.g., 192.168.1.x)");
}
Console.WriteLine("==================================================");

// إبقاء التطبيق قيد التشغيل
await app.WaitForShutdownAsync();