
Public Class Main
    Public path As String
    Public exePath As String
    Public appPath As String
    Public tarkovPath As String



    Private Sub SettingsTimer_Tick(sender As Object, e As EventArgs) Handles SettingsTimer.Tick
        SettingsTimer.Enabled = False
        Dim inipath As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Escape From Tarkov\local.ini"
        FileOpen(1, inipath, OpenMode.Binary)
        Lock(1)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        CheckForUpdates()
        CheckDirectory()
        'Me.Visible = False
        StartTimer.Enabled = True
    End Sub
    Private Sub StartTimer_Tick(sender As Object, e As EventArgs) Handles StartTimer.Tick
        StartTimer.Enabled = False
        Dim q() As Process
        q = Process.GetProcessesByName("EscapeFromTarkov")
        If q.Count > 0 Then
            ' Process is running

            MsgBox("Please close the game!", 16, "Error")
            Close()
        Else
            ' Process is not running
            '   Dim tarkovpath As String = "E:\Battlestate Games\BsgLauncher\BsgLauncher.exe"
            Dim proc As New System.Diagnostics.Process()
            proc = Process.Start(tarkovPath)
            TarkovTimer.Enabled = True
        End If
    End Sub
    Private Sub TarkovTimer_Tick(sender As Object, e As EventArgs) Handles TarkovTimer.Tick
        Dim p() As Process
        Dim z() As Process
        p = Process.GetProcessesByName("EscapeFromTarkov")
        z = Process.GetProcessesByName("BsgLauncher")
        If p.Count > 0 Then
            ' Process is running
            'MsgBox("Running!")
            TarkovTimer.Enabled = False
            ExitTimer.Enabled = True
            SettingsTimer.Enabled = True
        Else
            If z.Count > 0 Then
            Else
                Close()
            End If
        End If
    End Sub

    Private Sub ExitTimer_Tick(sender As Object, e As EventArgs) Handles ExitTimer.Tick
        Dim w() As Process
        w = Process.GetProcessesByName("EscapeFromTarkov")
        If w.Count > 0 Then
            ' Process is running
        Else
            ' Process is not running
            ExitTimer.Enabled = False
            Close()
        End If
    End Sub

    Public Sub CheckForUpdates()
        VersionLabel.Text = "v" & Application.ProductVersion & " ALPHA"
        exePath = Application.ExecutablePath()
        appPath = Application.StartupPath()
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\E4GL3\ETE", "exePath", exePath)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\E4GL3\ETE", "appPath", appPath)
        'MsgBox(appPath)

        Dim x As Integer
        Dim paths() As String = IO.Directory.GetFiles(appPath, "updatre.exe")
        If paths.Length > 0 Then
            For x = 0 To paths.Length - 1
                IO.File.Delete(paths(x))
            Next
        End If

        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://shieldspeak.com/eagle/version")
        Dim response As System.Net.HttpWebResponse = request.GetResponse()

        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())

        Dim newestversion As String = sr.ReadToEnd()
        Dim currentversion As String = Application.ProductVersion

        If newestversion.Contains(currentversion) Then
            ' MsgBox("No new updates")
        Else
            MsgBox("An update is available!")
            My.Computer.Network.DownloadFile("http://shieldspeak.com/eagle/Updatre.exe", appPath & "\updatre.exe")
            Process.Start("updatre.exe")
            Close()
        End If
    End Sub

    Public Sub CheckDirectory()
        tarkovPath = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\E4GL3\ETE", "tarkovPath", Nothing)
        If Not My.Computer.FileSystem.FileExists(tarkovPath) Then
            MsgBox("Please select your ""Battlestate Games"" folder")
            Dim folder As New FolderBrowserDialog
            Dim flag As Byte = 0
            '       Dim path As String
            Do
                If folder.ShowDialog = Windows.Forms.DialogResult.OK Then
                    'MsgBox(folder.SelectedPath)
                    path = folder.SelectedPath & "\BsgLauncher\BsgLauncher.exe"
                    ' MsgBox(path)
                    'Close()
                    If My.Computer.FileSystem.FileExists(path) Then
                        flag = 1
                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\E4GL3\ETE", "tarkovPath", path)
                    Else
                        MsgBox("Cannot find game launcher!", 16)
                        flag = 0
                        path = ""
                    End If

                End If
            Loop Until flag = 1
            tarkovPath = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\E4GL3\ETE", "tarkovPath", Nothing)

        End If

    End Sub

End Class
