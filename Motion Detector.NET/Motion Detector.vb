'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.Marshal
Imports System.Windows.Forms


'This module contains this program's core procedures.
Public Module MotionDetectorModule
   'The Microsoft Windows API constants, functions and structures used by this program.
   Public Const WM_CAP_DLG_VIDEOCOMPRESSION As Integer = 1070
   Public Const WM_CAP_DLG_VIDEOFORMAT As Integer = 1065
   Public Const WM_CAP_DLG_VIDEOSOURCE As Integer = 1066
   Private Const WM_CAP_DRIVER_CONNECT As Integer = 1034
   Private Const WM_CAP_DRIVER_DISCONNECT As Integer = 1035
   Private Const WM_CAP_EDIT_COPY As Integer = 1054
   Private Const WM_CAP_GET_STATUS As Integer = 1078
   Private Const WM_CAP_GRAB_FRAME As Integer = 1084
   Private Const WM_CLOSE As Integer = 16
   Private Const WS_CHILD As Integer = &H40000000%

   Private Structure POINTAPI
      Public x As Integer
      Public y As Integer
   End Structure

   Private Structure CAPSTATUS
      Public uiImageWidth As Integer
      Public uiImageHeight As Integer
      Public fLiveWindow As Integer
      Public fOverlayWindow As Integer
      Public fScale As Integer
      Public ptScroll As POINTAPI
      Public fUsingDefaultPalette As Integer
      Public fAudioHardware As Integer
      Public fCapFileExists As Integer
      Public dwCurrentVideoFrame As Integer
      Public dwCurrentVideoFramesDropped As Integer
      Public dwCurrentWaveSamples As Integer
      Public dwCurrentTimeElapsedMS As Integer
      Public hPalCurrent As IntPtr
      Public fCapturingNow As Integer
      Public dwReturn As Integer
      Public wNumVideoAllocated As Integer
      Public wNumAudioAllocated As Integer
   End Structure

   <DllImport("User32.dll", SetLastError:=True)> Public Function SendMessageA(ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
   End Function
   <DllImport("Avicap32.dll", SetLastError:=True)> Private Function capCreateCaptureWindowA(ByVal lpszWindowName As String, ByVal dwStyle As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hwndParent As IntPtr, ByVal nID As Integer) As IntPtr
   End Function

   'The structures and variables, used by this program.

   'This structure defines a bitmap's data.
   Public Structure BitmapDataStr
      Public BitmapDataO As BitmapData   'Defines a bitmap's data.
      Public PixelData() As Byte         'Defines a bitmap's pixel color values.
   End Structure

   Public ColorThreshold As Integer = 12    'Contains the difference threshold between a pixel's current and previous color.
   Public MotionThreshold As Integer = 10   'Contains the motion threshold above which a warning is triggered.

   'This procedure adjusts the specified picture box to the size of frames returned by the image capture device.
   Public Sub AdjustPositionAndSize(PictureBoxO As PictureBox, NewPosition As Point)
      Try
         Dim NewSize As Size = Nothing
         Dim Status As CAPSTATUS = GetCaptureStatus()

         With Status
            If .uiImageHeight = 0 OrElse .uiImageWidth = 0 Then
               NewSize = New Size(CInt(My.Computer.Screen.WorkingArea.Width / 2), CInt(My.Computer.Screen.WorkingArea.Height / 2))
            ElseIf .uiImageHeight >= My.Computer.Screen.WorkingArea.Width OrElse .uiImageWidth >= My.Computer.Screen.WorkingArea.height Then
               NewSize = New Size(CInt(My.Computer.Screen.WorkingArea.Width / 1.1), CInt(My.Computer.Screen.WorkingArea.Height / 1.1))
            Else
               NewSize = New Size(.uiImageWidth, .uiImageHeight)
            End If
         End With

         PictureBoxO.Location = NewPosition
         PictureBoxO.Size = NewSize
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub

   'This procedure manages the capture window.
   Public Function CaptureWindow(Optional StartCapture As Boolean = False, Optional StopCapture As Boolean = False) As IntPtr
      Try
         Static CaptureWindowH As New IntPtr

         If StartCapture Then
            CaptureWindowH = capCreateCaptureWindowA(Nothing, WS_CHILD, 0, 0, 0, 0, InterfaceWindow.Handle, 0)
            If Not CaptureWindowH = IntPtr.Zero Then SendMessageA(CaptureWindowH, WM_CAP_DRIVER_CONNECT, 0, Nothing)
         ElseIf StopCapture Then
            SendMessageA(CaptureWindowH, WM_CAP_DRIVER_DISCONNECT, 0, Nothing)
            SendMessageA(CaptureWindowH, WM_CLOSE, 0, Nothing)
            CaptureWindowH = IntPtr.Zero
         End If

         Return CaptureWindowH
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure returns the image capture device's status.
   Private Function GetCaptureStatus() As CAPSTATUS
      Try
         Dim Status As New CAPSTATUS
         Dim StatusH As IntPtr = AllocHGlobal(SizeOf(Status))

         SendMessageA(CaptureWindow(), WM_CAP_GET_STATUS, SizeOf(Status), StatusH)
         Status = DirectCast(PtrToStructure(StatusH, Status.GetType), CAPSTATUS)
         FreeHGlobal(StatusH)

         Return Status
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure returns a single frame from the image capture device.
   Public Function GrabFrame() As Bitmap
      Try
         SendMessageA(CaptureWindow(), WM_CAP_GRAB_FRAME, 0, Nothing)
         SendMessageA(CaptureWindow(), WM_CAP_EDIT_COPY, 0, Nothing)

         Return If(Clipboard.GetImage() Is Nothing, New Bitmap(InterfaceWindow.CurrentViewBox.Width, InterfaceWindow.CurrentViewBox.Height), New Bitmap(Clipboard.GetImage))
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure handles any errors that occur.
   Public Sub HandleError(ExceptionO As Exception)
      Dim Message As String = ExceptionO.Message

      Try
         InterfaceWindow.MotionDetector.Enabled = False
         If MessageBox.Show(Message, My.Application.Info.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = DialogResult.Cancel Then
            InterfaceWindow.Close()
         End If
         InterfaceWindow.MotionDetector.Enabled = True
      Catch
         [Exit](0)
      End Try
   End Sub

   'This procedure locks the specified bitmap and returns its data.
   Public Function LockBitmap(Source As Bitmap) As BitmapDataStr
      Try
         Dim BitmapDataO As New BitmapDataStr

         With BitmapDataO
            .BitmapDataO = Source.LockBits(New Rectangle(0, 0, Source.Width, Source.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)

            ReDim .PixelData(0 To .BitmapDataO.Stride * .BitmapDataO.Height)
            Copy(.BitmapDataO.Scan0, .PixelData, .PixelData.GetLowerBound(0), .PixelData.GetUpperBound(0))
         End With

         Return BitmapDataO
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure saves a snapshot.
   Public Function SaveSnapShot(SnapShot As Bitmap) As String
      Try
         Dim SnapShotFile As String = Path.Combine(My.Computer.FileSystem.CurrentDirectory, "Snapshot.bmp")

         SnapShot.Save(SnapShotFile)

         Return SnapShotFile
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try

      Return Nothing
   End Function

   'This procedure copies the specified pixel data to the specified bitmap and then unlocks it.
   Public Sub UnlockBitmap(Target As Bitmap, BitmapDataO As BitmapDataStr)
      Try
         With BitmapDataO
            Copy(.PixelData, 0, .BitmapDataO.Scan0, .BitmapDataO.Stride * .BitmapDataO.Height)
            Target.UnlockBits(.BitmapDataO)
         End With
      Catch ExceptionO As Exception
         HandleError(ExceptionO)
      End Try
   End Sub
End Module



