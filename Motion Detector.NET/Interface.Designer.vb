<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InterfaceWindow
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InterfaceWindow))
      Me.MenuBar = New System.Windows.Forms.MenuStrip()
      Me.ProgramMainMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.InformationMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.ProgramMainMenuSeparator1 = New System.Windows.Forms.ToolStripSeparator()
      Me.QuitMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.ViewMainMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.CurrentViewMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.MotionViewMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.OptionsMainMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.DisableWarningMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.MotionDetectionMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.OptionsMainMenuSeparator1 = New System.Windows.Forms.ToolStripSeparator()
      Me.VideoCompressionMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.VideoFormatMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.VideoSourceMenu = New System.Windows.Forms.ToolStripMenuItem()
      Me.CurrentViewBox = New System.Windows.Forms.PictureBox()
      Me.MotionViewBox = New System.Windows.Forms.PictureBox()
      Me.MenuBar.SuspendLayout()
      CType(Me.CurrentViewBox, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.MotionViewBox, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'MenuBar
      '
      Me.MenuBar.ImageScalingSize = New System.Drawing.Size(20, 20)
      Me.MenuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProgramMainMenu, Me.ViewMainMenu, Me.OptionsMainMenu})
      Me.MenuBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
      Me.MenuBar.Location = New System.Drawing.Point(0, 0)
      Me.MenuBar.Name = "MenuBar"
      Me.MenuBar.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
      Me.MenuBar.Size = New System.Drawing.Size(379, 28)
      Me.MenuBar.TabIndex = 0
      '
      'ProgramMainMenu
      '
      Me.ProgramMainMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InformationMenu, Me.ProgramMainMenuSeparator1, Me.QuitMenu})
      Me.ProgramMainMenu.Name = "ProgramMainMenu"
      Me.ProgramMainMenu.Size = New System.Drawing.Size(78, 24)
      Me.ProgramMainMenu.Text = "&Program"
      '
      'InformationMenu
      '
      Me.InformationMenu.Name = "InformationMenu"
      Me.InformationMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
      Me.InformationMenu.Size = New System.Drawing.Size(208, 26)
      Me.InformationMenu.Text = "&Information"
      '
      'ProgramMainMenuSeparator1
      '
      Me.ProgramMainMenuSeparator1.Name = "ProgramMainMenuSeparator1"
      Me.ProgramMainMenuSeparator1.Size = New System.Drawing.Size(205, 6)
      '
      'QuitMenu
      '
      Me.QuitMenu.Name = "QuitMenu"
      Me.QuitMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
      Me.QuitMenu.Size = New System.Drawing.Size(208, 26)
      Me.QuitMenu.Text = "&Quit"
      '
      'ViewMainMenu
      '
      Me.ViewMainMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CurrentViewMenu, Me.MotionViewMenu})
      Me.ViewMainMenu.Name = "ViewMainMenu"
      Me.ViewMainMenu.Size = New System.Drawing.Size(53, 24)
      Me.ViewMainMenu.Text = "&View"
      '
      'CurrentViewMenu
      '
      Me.CurrentViewMenu.Name = "CurrentViewMenu"
      Me.CurrentViewMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
      Me.CurrentViewMenu.Size = New System.Drawing.Size(223, 26)
      Me.CurrentViewMenu.Text = "&Current View"
      '
      'MotionViewMenu
      '
      Me.MotionViewMenu.Name = "MotionViewMenu"
      Me.MotionViewMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
      Me.MotionViewMenu.Size = New System.Drawing.Size(223, 26)
      Me.MotionViewMenu.Text = "&Motion View"
      '
      'OptionsMainMenu
      '
      Me.OptionsMainMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisableWarningMenu, Me.MotionDetectionMenu, Me.OptionsMainMenuSeparator1, Me.VideoCompressionMenu, Me.VideoFormatMenu, Me.VideoSourceMenu})
      Me.OptionsMainMenu.Name = "OptionsMainMenu"
      Me.OptionsMainMenu.Size = New System.Drawing.Size(73, 24)
      Me.OptionsMainMenu.Text = "&Options"
      '
      'DisableWarningMenu
      '
      Me.DisableWarningMenu.Name = "DisableWarningMenu"
      Me.DisableWarningMenu.ShortcutKeyDisplayString = "Ctrl+W"
      Me.DisableWarningMenu.Size = New System.Drawing.Size(264, 26)
      Me.DisableWarningMenu.Text = "&Disable Warning"
      '
      'MotionDetectionMenu
      '
      Me.MotionDetectionMenu.Name = "MotionDetectionMenu"
      Me.MotionDetectionMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
      Me.MotionDetectionMenu.Size = New System.Drawing.Size(264, 26)
      Me.MotionDetectionMenu.Text = "Motion &Detection"
      '
      'OptionsMainMenuSeparator1
      '
      Me.OptionsMainMenuSeparator1.Name = "OptionsMainMenuSeparator1"
      Me.OptionsMainMenuSeparator1.Size = New System.Drawing.Size(261, 6)
      '
      'VideoCompressionMenu
      '
      Me.VideoCompressionMenu.Name = "VideoCompressionMenu"
      Me.VideoCompressionMenu.ShortcutKeyDisplayString = "Ctrl+V"
      Me.VideoCompressionMenu.Size = New System.Drawing.Size(264, 26)
      Me.VideoCompressionMenu.Text = "Video &Compression"
      '
      'VideoFormatMenu
      '
      Me.VideoFormatMenu.Name = "VideoFormatMenu"
      Me.VideoFormatMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
      Me.VideoFormatMenu.Size = New System.Drawing.Size(264, 26)
      Me.VideoFormatMenu.Text = "Video &Format"
      '
      'VideoSourceMenu
      '
      Me.VideoSourceMenu.Name = "VideoSourceMenu"
      Me.VideoSourceMenu.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
      Me.VideoSourceMenu.Size = New System.Drawing.Size(264, 26)
      Me.VideoSourceMenu.Text = "Video &Source"
      '
      'CurrentViewBox
      '
      Me.CurrentViewBox.Anchor = System.Windows.Forms.AnchorStyles.None
      Me.CurrentViewBox.BackColor = System.Drawing.SystemColors.Control
      Me.CurrentViewBox.Location = New System.Drawing.Point(0, 33)
      Me.CurrentViewBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
      Me.CurrentViewBox.Name = "CurrentViewBox"
      Me.CurrentViewBox.Size = New System.Drawing.Size(69, 57)
      Me.CurrentViewBox.TabIndex = 1
      Me.CurrentViewBox.TabStop = False
      Me.CurrentViewBox.Visible = False
      '
      'MotionViewBox
      '
      Me.MotionViewBox.Location = New System.Drawing.Point(0, 33)
      Me.MotionViewBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
      Me.MotionViewBox.Name = "MotionViewBox"
      Me.MotionViewBox.Size = New System.Drawing.Size(123, 100)
      Me.MotionViewBox.TabIndex = 2
      Me.MotionViewBox.TabStop = False
      Me.MotionViewBox.Visible = False
      '
      'InterfaceWindow
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
      Me.ClientSize = New System.Drawing.Size(379, 322)
      Me.Controls.Add(Me.CurrentViewBox)
      Me.Controls.Add(Me.MenuBar)
      Me.Controls.Add(Me.MotionViewBox)
      Me.DoubleBuffered = True
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.KeyPreview = True
      Me.MainMenuStrip = Me.MenuBar
      Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
      Me.MaximizeBox = False
      Me.Name = "InterfaceWindow"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.MenuBar.ResumeLayout(False)
      Me.MenuBar.PerformLayout()
      CType(Me.CurrentViewBox, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.MotionViewBox, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents MenuBar As System.Windows.Forms.MenuStrip
   Friend WithEvents ProgramMainMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents InformationMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents ProgramMainMenuSeparator1 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents QuitMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents ViewMainMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents CurrentViewMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents MotionViewMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents OptionsMainMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents DisableWarningMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents MotionDetectionMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents OptionsMainMenuSeparator1 As System.Windows.Forms.ToolStripSeparator
   Friend WithEvents VideoCompressionMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents VideoFormatMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents VideoSourceMenu As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents CurrentViewBox As System.Windows.Forms.PictureBox
   Friend WithEvents MotionViewBox As System.Windows.Forms.PictureBox

End Class
