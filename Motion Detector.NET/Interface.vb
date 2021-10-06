'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Environment
Imports System.Linq
Imports System.Math
Imports System.Media
Imports System.Windows.Forms

'This module contains this program's main interface window.
Public Class InterfaceWindow
   'This enumaration contains the list of the available views.
   Private Enum ViewsE As Integer
      CurrentView      'Current view.
      MotionView       'Motion viewer.
   End Enum

   'This structure defines a RGB color.
   Private Structure RGBStr
      Public Red As Integer    'Defines the red color component.
      Public Green As Integer  'Defines the green color component.
      Public Blue As Integer   'Defines the blue color component.
   End Structure

   Private Const BYTES_PER_PIXEL As Integer = 3  'The number of bytes per RGB pixel.

   Public WithEvents MotionDetector As New Timer With {.Enabled = False, .Interval = 1}  'Contains the motion detector.

   'This procedure displays a warning when motion is detected.
   Private Sub DisplayWarning(MotionLevel As Integer, SnapShotFile As String)
      Try
         My.Computer.Audio.PlaySystemSound(SystemSounds.Beep)

         MotionDetector.Enabled = False
         DisableWarningMenu.Checked = (MessageBox.Show($"Motion (level {MotionLevel}) detected.{NewLine}Time: {My.Computer.Clock.LocalTime}{NewLine}Continue displaying warnings?", My.Application.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.No)
         MotionDetector.Enabled = True
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure selects the specified view.
   Private Sub SelectView(NewView As ViewsE)
      Try
         If NewView = ViewsE.CurrentView Then
            CurrentViewMenu.Checked = True
            MotionViewMenu.Checked = False

            CurrentViewBox.Visible = True
            MotionViewBox.Visible = False
         ElseIf NewView = ViewsE.MotionView Then
            CurrentViewMenu.Checked = False
            MotionViewMenu.Checked = True

            CurrentViewBox.Visible = False
            MotionViewBox.Visible = True
         End If
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure adjusts this window to the new size of the current view picture box.
   Private Sub CurrentViewBox_Resize(sender As Object, e As EventArgs) Handles CurrentViewBox.Resize
      Try
         Me.ClientSize = CurrentViewBox.Size
         Me.Height += MenuBar.Height
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command change the view to the user's selection.
   Private Sub CurrentViewMenu_Click(sender As Object, e As EventArgs) Handles CurrentViewMenu.Click
      Try
         SelectView(ViewsE.CurrentView)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure disabled/enables motion warnings.
   Private Sub DisableWarningMenu_Click(sender As Object, e As EventArgs) Handles DisableWarningMenu.Click
      Try
         DisableWarningMenu.Checked = Not DisableWarningMenu.Checked
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure displays information about this program.
   Private Sub InformationMenu_Click(sender As Object, e As EventArgs) Handles InformationMenu.Click
      Try
         With My.Application.Info
            MessageBox.Show(.Description, $"{ .Title} v{ .Version} - by: { .CompanyName}", MessageBoxButtons.OK, MessageBoxIcon.Information)
         End With
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command to stop the image capture device.
   Private Sub InterfaceWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
      Try
         CaptureWindow(, StopCapture:=True)
         MotionDetector.Enabled = False
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure initializes this window.
   Private Sub InterfaceWindow_Load(sender As Object, e As EventArgs) Handles Me.Load
      Try
         Me.Text = My.Application.Info.Title

         SelectView(ViewsE.MotionView)

         If CaptureWindow(StartCapture:=True) = IntPtr.Zero Then Me.Close()

         AdjustPositionAndSize(CurrentViewBox, New Point(0, MenuBar.Height))
         AdjustPositionAndSize(MotionViewBox, New Point(0, MenuBar.Height))

         MotionDetector.Enabled = True
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure adjusts the window and its controls to its new size.
   Private Sub InterfaceWindow_Resize(sender As Object, e As EventArgs) Handles Me.Resize
      Try
         With My.Computer.Screen.WorkingArea
            Me.Left = CInt(.Width / 2) - CInt(Me.Width / 2)
            Me.Top = CInt(.Height / 2) - CInt(Me.Height / 2)
         End With
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure requests the user to specify the motion detection related settings.
   Private Sub MotionDetectionMenu_Click(sender As Object, e As EventArgs) Handles MotionDetectionMenu.Click
      Try
         Dim NewColorThreshold As Integer = Nothing
         Dim NewMotionThreshold As Integer = Nothing

         Integer.TryParse(InputBox("Color difference threshold (1-255)", , CStr(ColorThreshold)), NewColorThreshold)
         Integer.TryParse(InputBox("Warning motion threshold (1-100)", , CStr(MotionThreshold)), NewMotionThreshold)

         If NewColorThreshold >= 1 AndAlso NewColorThreshold <= 255 Then ColorThreshold = NewColorThreshold
         If NewMotionThreshold >= 1 AndAlso NewMotionThreshold <= 100 Then MotionThreshold = NewMotionThreshold
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure manages the motion detector.
   Private Sub MotionDetector_Tick(sender As Object, e As EventArgs) Handles MotionDetector.Tick
      Try
         Dim CurrentData As BitmapDataStr = Nothing
         Dim Difference As New RGBStr
         Dim DifferenceCount As New Integer
         Dim DifferenceData As BitmapDataStr = Nothing
         Dim DifferenceView As Bitmap = Nothing
         Dim MotionLevel As Integer = Nothing
         Dim PreviousData As BitmapDataStr = Nothing
         Static CurrentView As Bitmap = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)
         Static PreviousView As Bitmap = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)

         Try
            DifferenceView = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)
            PreviousView = CurrentView
            CurrentView = GrabFrame()
            CurrentViewBox.Image = CurrentView

            CurrentData = LockBitmap(CurrentView)
            DifferenceData = LockBitmap(DifferenceView)
            PreviousData = LockBitmap(PreviousView)

            With DifferenceData.PixelData
               DifferenceCount = 0
               For Index As Integer = .GetLowerBound(0) To .GetUpperBound(0) - 2 Step BYTES_PER_PIXEL
                  Difference.Red = Abs(CInt(CurrentData.PixelData(Index) - CInt(PreviousData.PixelData(Index))))
                  Difference.Green = Abs(CInt(CurrentData.PixelData(Index + 1) - CInt(PreviousData.PixelData(Index + 1))))
                  Difference.Blue = Abs(CInt(CurrentData.PixelData(Index + 2) - CInt(PreviousData.PixelData(Index + 2))))

                  If {Difference.Red, Difference.Green, Difference.Blue}.Average > ColorThreshold Then
                     DifferenceData.PixelData(Index) = Byte.MaxValue
                     DifferenceData.PixelData(Index + 1) = Byte.MaxValue
                     DifferenceData.PixelData(Index + 2) = Byte.MaxValue
                     DifferenceCount += 1
                  Else
                     DifferenceData.PixelData(Index) = Byte.MinValue
                     DifferenceData.PixelData(Index + 1) = Byte.MinValue
                     DifferenceData.PixelData(Index + 2) = Byte.MinValue
                  End If
               Next Index
            End With

            UnlockBitmap(CurrentView, CurrentData)
            UnlockBitmap(DifferenceView, DifferenceData)
            UnlockBitmap(PreviousView, PreviousData)

            MotionViewBox.Image = DifferenceView

            MotionLevel = CInt((100 / (CurrentView.Width * CurrentView.Height)) * DifferenceCount)
            Me.Text = $"{My.Application.Info.Title} - Motion level: {MotionLevel} - Threshold: {MotionThreshold} - Color Difference Threshold: {ColorThreshold}"

            If MotionLevel >= MotionThreshold AndAlso Not DisableWarningMenu.Checked Then
               DisplayWarning(MotionLevel, SaveSnapShot(CurrentView))
               CurrentView = GrabFrame()
               Me.MotionViewBox.Image = CurrentView
               PreviousView = CurrentView
            End If
         Catch
            Try
               CurrentData = Nothing
               CurrentView = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)
               Difference = Nothing
               DifferenceCount = Nothing
               DifferenceData = Nothing
               DifferenceView = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)
               MotionLevel = Nothing
               PreviousData = Nothing
               PreviousView = New Bitmap(CurrentViewBox.Width, CurrentViewBox.Height)

               AdjustPositionAndSize(CurrentViewBox, New Point(0, MenuBar.Height))
               AdjustPositionAndSize(MotionViewBox, New Point(0, MenuBar.Height))
            Catch ExceptionO As Exception
               HandleError(ExceptionO)
            End Try
         End Try
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure gives the command change the view to the user's selection.
   Private Sub MotionViewMenu_Click(sender As Object, e As EventArgs) Handles MotionViewMenu.Click
      Try
         SelectView(ViewsE.MotionView)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure closes this window.
   Private Sub QuitMenu_Click(sender As Object, e As EventArgs) Handles QuitMenu.Click
      Try
         Me.Close()
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure opens the video compression dialog window.
   Private Sub VideoCompressionMenu_Click(sender As Object, e As EventArgs) Handles VideoCompressionMenu.Click
      Try
         SendMessageA(CaptureWindow(), WM_CAP_DLG_VIDEOCOMPRESSION, 0, Nothing)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure opens the video format dialog window.
   Private Sub VideoFormatMenu_Click(sender As Object, e As EventArgs) Handles VideoFormatMenu.Click
      Try
         MotionDetector.Enabled = False
         SendMessageA(CaptureWindow(), WM_CAP_DLG_VIDEOFORMAT, 0, Nothing)
         AdjustPositionAndSize(CurrentViewBox, New Point(0, MenuBar.Height))
         MotionDetector.Enabled = True
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure opens the video source dialog window.
   Private Sub VideoSourceMenu_Click(sender As Object, e As EventArgs) Handles VideoSourceMenu.Click
      Try
         SendMessageA(CaptureWindow(), WM_CAP_DLG_VIDEOSOURCE, 0, Nothing)
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub
End Class
