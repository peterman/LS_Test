Imports System
Imports System.IO
Imports System.Collections

Public Class Main

    Dim Testcycle,
        Teststep,
        Testcount,
        Testcountmax As Integer
    Dim SerPort As Array


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Splash.Show()

        Init()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Test_start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Test_stop()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case Teststep
            Case 1                                  'Betriebsart Messen
                RadioButton2.Checked = True
                If Testcount = 0 Then
                    Teststep = 2
                    Testcount = Val(ComboBox2.SelectedItem)
                    Testcountmax = Testcount
                    command("Stop")
                End If
            Case 2                                  'Betriebsart Standby ohne Vent
                RadioButton3.Checked = True
                If Testcount = 0 Then
                    Teststep = 3
                    Testcount = Val(ComboBox3.SelectedItem)
                    Testcountmax = Testcount
                    command("Vent")
                End If
            Case 3                                  'Betriebsart Vent
                RadioButton4.Checked = True
                If Testcount = 0 Then
                    If Testcycle = 0 Then
                        Teststep = 4
                        Testcount = Val(ComboBox4.SelectedItem)
                        Testcountmax = Testcount
                        command("Cal")
                    Else
                        Teststep = 1
                        Testcount = Val(ComboBox1.SelectedItem)
                        Testcountmax = Testcount
                        Testcycle -= 1
                        command("start")
                    End If
                End If
            Case 4
                RadioButton5.Checked = True
                If Testcount = 0 Then
                    Teststep = 1
                    Testcount = Val(ComboBox1.SelectedItem)
                    Testcountmax = Testcount
                    command("start")
                End If
        End Select
        ToolStripProgressBar1.Value = (Testcount / Testcountmax) * 100
        Testcount -= 1
        TextBox6.Text = Testcount

    End Sub

    Private Sub Init()
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        Timer1.Interval = 100
        SerPort = IO.Ports.SerialPort.GetPortNames
        ComboBox5.Items.AddRange(SerPort)
    End Sub

    Private Sub Test_start()
        ComboBox1.Enabled = False
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        ComboBox4.Enabled = False
        SerialPort1.PortName = ComboBox1.Text
        SerialPort1.BaudRate = 9600
        SerialPort1.Parity = IO.Ports.Parity.None
        SerialPort1.StopBits = IO.Ports.StopBits.One
        SerialPort1.DataBits = 8
        SerialPort1.Open()
        ComboBox5.Enabled = False
        Teststep = 1
        Testcycle = 1
        Testcount = Val(ComboBox1.SelectedItem)
        Testcountmax = Testcount

        Timer1.Enabled = True
        Timer1.Start()

        TextBox6.Text = Testcount
        command("start")
    End Sub

    Private Sub Test_stop()
        Timer1.Enabled = False
        ComboBox1.Enabled = True
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
        ComboBox4.Enabled = True
        ComboBox5.Enabled = True
        Timer1.Stop()

    End Sub

    Private Sub command(cmd As String)
        Select Case cmd.ToLower

            Case "start"
                ToolStripStatusLabel3.Text = "Start Messen"
            Case "stop"
                ToolStripStatusLabel3.Text = "Stop Messen"
            Case "cal"
                ToolStripStatusLabel3.Text = "Start Kalibrieren"
            Case "vent"
                ToolStripStatusLabel3.Text = "Belüften"
        End Select
    End Sub

    Private Sub SendRS232(cmd As String)
        Select Case cmd.ToLower
            Case "start"
                'writeRS232 = "=CYE" & vbcr
            Case "stop"
                'writeRS232 = "=CYD" & vbcr
            Case "cal"
                'writeRS232 = "!AC" & vbcr
            Case "vent"
                'writeRS232 = "=IVE" & vbcr
            Case "status"
                'writeRS232 = "?TR" & vbcr
        End Select
    End Sub


End Class
