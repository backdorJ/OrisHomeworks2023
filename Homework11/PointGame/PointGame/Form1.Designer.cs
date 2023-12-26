namespace PointGame;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btn_signIn = new Button();
        label1 = new Label();
        enterName = new TextBox();
        listOfUsers = new ListView();
        name = new Label();
        color = new Label();
        SuspendLayout();
        // 
        // btn_signIn
        // 
        btn_signIn.Location = new Point(308, 170);
        btn_signIn.Margin = new Padding(3, 2, 3, 2);
        btn_signIn.Name = "btn_signIn";
        btn_signIn.Size = new Size(82, 22);
        btn_signIn.TabIndex = 0;
        btn_signIn.Text = "Войти";
        btn_signIn.UseVisualStyleBackColor = true;
        btn_signIn.Click += btn_signIn_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(295, 115);
        label1.Name = "label1";
        label1.Size = new Size(110, 15);
        label1.TabIndex = 1;
        label1.Text = "Введите ваше имя!";
        // 
        // enterName
        // 
        enterName.Location = new Point(295, 143);
        enterName.Margin = new Padding(3, 2, 3, 2);
        enterName.Name = "enterName";
        enterName.Size = new Size(110, 23);
        enterName.TabIndex = 2;
        // 
        // listOfUsers
        // 
        listOfUsers.View = View.List;
        listOfUsers.Location = new Point(545, 60);
        listOfUsers.Margin = new Padding(3, 2, 3, 2);
        listOfUsers.Name = "listOfUsers";
        listOfUsers.Size = new Size(126, 274);
        listOfUsers.TabIndex = 3;
        // 
        // name
        // 
        name.AutoSize = true;
        name.Location = new Point(562, 23);
        name.Name = "name";
        name.Size = new Size(0, 15);
        name.TabIndex = 4;
        // 
        // color
        // 
        color.AutoSize = true;
        color.Location = new Point(637, 23);
        color.Name = "color";
        color.Size = new Size(34, 15);
        color.TabIndex = 5;
        color.Text = "color";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(692, 364);
        Controls.Add(color);
        Controls.Add(name);
        Controls.Add(listOfUsers);
        Controls.Add(enterName);
        Controls.Add(label1);
        Controls.Add(btn_signIn);
        Margin = new Padding(3, 2, 3, 2);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btn_signIn;
    private Label label1;
    private TextBox enterName;
    private ListView listOfUsers;
    private Label name;
    private Label color;
}