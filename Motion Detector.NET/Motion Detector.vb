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
   Public Const WM_CAP_DLG_VIDEOCOMPRESSION As UInteger = &H42EUI
   Public Const WM_CAP_DLG_VIDEOFORMAT As UInteger = &H42CUI
   Public Const WM_CAP_DLG_VIDEOSOURCE As UInteger = &H42AUI
   Private Const WM_CAP_DRIVER_CONNECT As UInteger = &H40AUI
   Private Const WM_CAP_DRIVER_DISCONNECT As UInteger = &H40BUI
   Private Const WM_CAP_EDIT_COPY As UInteger = &H41EUI
   Private Const WM_CAP_GET_STATUS As UInteger = &H436UI
   Private Const WM_CAP_GRAB_FRAME As UInteger = &H43CUI
   Private Const WM_CLOSE As UInteger = &H10UI
   Private Const WS_CHILD As UInteger = &H40000000UI

   <StructLayout(LayoutKind.Sequential)>
   Private Structure POINTAPI
      Public x As Integer
      Public y As Integer
   End Structure

   <StructLayout(LayoutKind.Sequential)>
   Private Structure CAPSTATUS
      Public uiImageWidth As UInteger
      Public uiImageHeight As UInteger
      Public fLiveWindow As Boolean
      Public fOverlayWindow As Boolean
      Public fScale As Boolean
      Public ptScroll As POINTAPI
      Public fUsingDefaultPalette As Boolean
      Public fAudioHardware As Boolean
      Public fCapFileExists As Boolean
      Public dwCurrentVideoFrame As UInteger
      Public dwCurrentVideoFramesDropped As UInteger
      Public dwCurrentWaveSamples As UInteger
      Public dwCurrentTimeElapsedMS As UInteger
      Public hPalCurrent As IntPtr
      Public fCapturingNow As Boolean
      Public dwReturn As UInteger
      Public wNumVideoAllocated As UInteger
      Public wNumAudioAllocated As UInteger
   End Structure

   <DllImport("User32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> Public Function SendMessageA(ByVal hWnd As IntPtr, ByVal wMsg As UInteger, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
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
               NewSize = New Size(CInt(.uiImageWidth), CInt(.uiImageHeight))
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
            If Not CaptureWindowH = IntPtr.Zero Then SendMessageA(CaptureWindowH, WM_CAP_DRIVER_CONNECT, Nothing, IntPtr.Zero)
         ElseIf StopCapture Then
            SendMessageA(CaptureWindowH, WM_CAP_DRIVER_DISCONNECT, Nothing, IntPtr.Zero)
            SendMessageA(CaptureWindowH, WM_CLOSE, Nothing, IntPtr.Zero)
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
         SendMessageA(CaptureWindow(), WM_CAP_GRAB_FRAME, Nothing, IntPtr.Zero)
         SendMessageA(CaptureWindow(), WM_CAP_EDIT_COPY, Nothing, IntPtr.Zero)

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



