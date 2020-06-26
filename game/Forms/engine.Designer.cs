
partial class engine
{
    /// <summary>
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
        this.rendererBox = new RendererBox();
        this.SuspendLayout();
        // 
        // rendererBox
        // 
        this.rendererBox.BackColor = System.Drawing.SystemColors.WindowText;
        this.rendererBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.rendererBox.Location = new System.Drawing.Point(0, 0);
        this.rendererBox.Name = "rendererBox";
        this.rendererBox.Size = new System.Drawing.Size(1264, 681);
        this.rendererBox.TabIndex = 0;
        // 
        // engine
        // 
        this.ClientSize = new System.Drawing.Size(1264, 681);
        this.Controls.Add(this.rendererBox);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        this.Name = "engine";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
        this.Load += new System.EventHandler(this.OnLoad);
        this.ResumeLayout(false);

    }

    #endregion

    private RendererBox rendererBox;
}