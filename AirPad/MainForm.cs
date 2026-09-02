using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace AirPad;

public class MainForm : Form
{
    private NotifyIcon _trayIcon;

    public MainForm(string url)
    {
        Icon appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        this.Text = "AirPad";
        this.Icon = appIcon;
        this.Size = new Size(350, 450);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        // تسمية الرابط
        var lblUrl = new Label
        {
            Text = $"امسح الكود للاتصال\n{url}",
            Dock = DockStyle.Top,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Arial", 10, FontStyle.Bold)
        };
        this.Controls.Add(lblUrl);

        // توليد الـ QR Code كصورة
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrImage = qrCode.GetGraphic(5);

            var pictureBox = new PictureBox
            {
                Image = qrImage,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(pictureBox);
        }

        // إعداد أيقونة بجوار الساعة (System Tray)
        _trayIcon = new NotifyIcon()
        {
            Icon = appIcon, // تغيير أيقونة System Tray
            Visible = true,
            Text = "AirPad"
        };

        // قائمة الخيارات عند الضغط كليك يمين على الأيقونة
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("إظهار الـ QR Code", null, (s, e) => this.Show());
        contextMenu.Items.Add("إغلاق السيرفر", null, (s, e) => Application.Exit());
        _trayIcon.ContextMenuStrip = contextMenu;

        // إخفاء النافذة عند الضغط على X بدلاً من إغلاق البرنامج
        this.FormClosing += (s, e) =>
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _trayIcon.Dispose();
        base.Dispose(disposing);
    }
}