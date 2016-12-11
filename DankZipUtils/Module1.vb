Imports Ionic
Imports System.Threading

Module Module1

    Dim ya As Boolean = False
    Dim ia As Boolean = False
    Dim da As Boolean = False
    Dim ea As Boolean = False
    Dim aa As Boolean = False
    Dim ua As Boolean = False
    Dim la As Boolean = False
    Dim na As Boolean = False
    Dim wa As Boolean = False
    Dim ThrEnd As Integer = 0
    Dim zipfile As String = ""
    Dim passlng As Integer = 4
    Dim nthreads As Integer = 4
    Dim search As Boolean = True
    Dim found As Boolean = True
    Dim passwd As String = ""
    Dim dict As String = ""

    Sub Main(args As String())
        Console.WriteLine("")
        If args.Length = 0 Then
            help()
        End If

        For i As Integer = 0 To args.Length - 1
            If args(i) = "-h" Then
                help()
                End
            End If
            If args(i) = "-n" Then
                na = True
            End If
            If args(i) = "-l" Then
                la = True
            End If
            If args(i) = "-u" Then
                ua = True
            End If
            If args(i) = "-a" Then
                aa = True
            End If
            If args(i) = "-e" Then
                ea = True
            End If
            If args(i) = "-d" Then
                da = True
            End If
            If args(i) = "-i" Then
                ia = True
            End If
            If args(i) = "-y" Then
                ya = True
            End If
            If args(i) = "-z" Then
                i = i + 1
                zipfile = args(i)
            End If
            If args(i) = "-w" Then
                i = i + 1
                dict = args(i)
                wa = True
            End If
            If args(i) = "-c" Then
                i = i + 1
                Try
                    passlng = args(i)
                Catch ex As Exception
                    Console.WriteLine("ERROR: '" & args(i) & "' is not a number.")
                    End
                End Try
            End If
            If args(i) = "-t" Then
                i = i + 1
                Try
                    nthreads = args(i)
                Catch ex As Exception
                    Console.WriteLine("ERROR: '" & args(i) & "' is not a number.")
                    End
                End Try
            End If
            If args(i) = "-o" Then
                i = i + 1
                zipfile = args(i)
            End If
        Next

        If na = False And la = False And ua = False And ea = False And da = False And ia = False And ya = False And aa = False Then
            Console.WriteLine("So... what do you want me to do :) ?")
            End
        End If

        If zipfile = "" Then
            Console.WriteLine("ERROR: Please entre a zip file.")
        Else
            If na = True Then
                If passlng > 0 Then
                    Try
                        If nthreads >= 1 Then
                            Console.WriteLine("Trying to crack " & zipfile & " using only numbers, " & nthreads & " threads and a " & passlng & " digit long password !")
                            Console.WriteLine("")
                            Dim testPerThread = Math.Round("".PadLeft(passlng, "9") / nthreads) + 1
                            search = True
                            ThrEnd = 0
                            found = False
                            For i As Integer = 0 To nthreads - 1
                                Dim t As New Thread(AddressOf crack)
                                Dim p As New CrackParams
                                p.startI = testPerThread * i
                                p.endI = testPerThread * i + testPerThread
                                p.methode = "a"
                                Console.WriteLine("Starting thread " & i & " (" & testPerThread * i & " --> " & testPerThread * i + testPerThread & ")")
                                t.Start(p)
                            Next
                            Console.WriteLine("")

                            While found = False And ThrEnd < nthreads
                                Thread.Sleep(500)
                                Console.WriteLine("Cracking...")
                            End While
                            Console.WriteLine("")
                            If found = True Then
                                found = False
                                Console.WriteLine("Done. password is " & passwd & "!")
                            Else
                                Console.WriteLine("Done. password not found :(")
                            End If
                        Else
                            Console.WriteLine("ERROR: You cannot crack a zip with less than 1 thread...")
                        End If

                    Catch ex As Exception

                    End Try
                Else

                End If
            End If

            If la = True Then
                For i As Integer = 0 To 100
                    Console.WriteLine(getPass(i, "A"))
                Next
                End
                If passlng > 0 Then
                    Try
                        If nthreads >= 1 Then
                            Console.WriteLine("Trying to crack " & zipfile & " using only numbers, " & nthreads & " threads and a " & passlng & " digit long password !")
                            Console.WriteLine("")
                            Dim testPerThread = Math.Round(Math.Pow(26, passlng) / nthreads) + 1
                            search = True
                            ThrEnd = 0
                            found = False
                            For i As Integer = 0 To nthreads - 1
                                Dim t As New Thread(AddressOf crack)
                                Dim p As New CrackParams
                                p.startI = testPerThread * i
                                p.endI = testPerThread * i + testPerThread
                                p.methode = "b"
                                Console.WriteLine("Starting thread " & i & " (" & getPass(testPerThread * i, "A") & " --> " & getPass(testPerThread * i + testPerThread, "A") & ")")
                                t.Start(p)
                            Next
                            Console.WriteLine("")

                            While found = False And ThrEnd < nthreads
                                Thread.Sleep(500)
                                Console.WriteLine("Cracking...")
                            End While
                            Console.WriteLine("")
                            If found = True Then
                                found = False
                                Console.WriteLine("Done. password is " & getPass(passwd, "A") & "!")
                            Else
                                Console.WriteLine("Done. password not found :(")
                            End If
                        Else
                            Console.WriteLine("ERROR: You cannot crack a zip with less than 1 thread...")
                        End If

                    Catch ex As Exception

                    End Try
                Else

                End If
            End If

        End If

    End Sub

    Sub crack(ByVal p As Object)
        Dim startI As Long = p.startI
        Dim endI As Long = p.endI
        Dim methode As Char = p.methode
        If methode = "a" Then
            If endI = -1 Then
                Dim i As Integer = 0
                While found = False And search = True
                    Try
                        If Ionic.Zip.ZipFile.CheckZipPassword(zipfile, i) Then
                            search = False
                            found = True
                            passwd = i
                        End If
                        Console.WriteLine(i)
                    Catch ex As Exception

                    End Try
                    i = i + 1
                End While
                ThrEnd = ThrEnd + 1
            Else
                For i As Integer = startI To endI

                    Try
                        If search = False Or found = True Then
                            i = endI + 1
                        End If
                        If Ionic.Zip.ZipFile.CheckZipPassword(zipfile, i) Then
                            search = False
                            found = True
                            passwd = i
                            ThrEnd = ThrEnd + 1
                            i = endI + 1
                        End If
                    Catch ex As Exception

                    End Try
                Next
                ThrEnd = ThrEnd + 1
            End If
        ElseIf methode = "b" Then

            For i As Integer = startI To endI

                Try
                    If search = False Or found = True Then
                        i = endI + 1
                    End If
                    If Ionic.Zip.ZipFile.CheckZipPassword(zipfile, getPass(i, "A")) Then
                        search = False
                        found = True
                        passwd = i
                        ThrEnd = ThrEnd + 1
                        i = endI + 1

                    End If
                    Console.WriteLine(getPass(i, "A"))
                Catch ex As Exception

                End Try
            Next
            ThrEnd = ThrEnd + 1

        End If
    End Sub

    Sub help()
        Console.WriteLine("╔═════════════════════════════════════════════╗")
        Console.WriteLine("║**°°       ┌─┐       ┌┐     ┬┐     ┬  ┬    ┬ ║")
        Console.WriteLine("║°°**°°     │ └─┐    ┌┘└┐    │└┐    │  │    │ ║")
        Console.WriteLine("║  °°**°°   │   └┐   │  │    │ └┐   │  │  ┌─┘ ║")
        Console.WriteLine("║    °°**°° │    │  ┌┘  └┐   │  └┐  │  │┌─┘   ║")
        Console.WriteLine("║    °°**°° │    │  │    │   │   └┐ │  ├┤     ║")
        Console.WriteLine("║  °°**°°   │   ┌┘ ┌┴────┴┐  │    └┐│  │└─┐   ║")
        Console.WriteLine("║°°**°°     │ ┌─┘  │      │  │     └┤  │  └─┐ ║")
        Console.WriteLine("║**°°       └─┘    ┴      ┴  ┴      ┴  ┴    ┴ ║")
        Console.WriteLine("╚═════════════════════════════════════════════╝")
        Console.WriteLine("DankZipUtils by Alexandre Rouma")
        Console.WriteLine("")
        Console.WriteLine("Please use this program responsibly, I create it to help people")
        Console.WriteLine("with their zip files, not for cracking other people's files.")
        Console.WriteLine("This program is for personal use ONLY. Please contact me if you")
        Console.WriteLine("wish to use this program for comercial perposes.")
        Console.WriteLine("")
        Console.WriteLine("Syntax: dankziputils [options]")
        Console.WriteLine("")
        Console.WriteLine("Options:")
        Console.WriteLine("         -z [filename]       Zip file")
        Console.WriteLine("         -c [number]         Password length limit (default is 4)")
        Console.WriteLine("         -t [number]         Number of threads used to crack the zip (Default is 4)")
        Console.WriteLine("         -n                  Crack zip using numbers")
        Console.WriteLine("         -l                  Crack zip using Lower Case letters")
        Console.WriteLine("         -u                  Crack zip using Upper Case letters")
        Console.WriteLine("         -a                  Crack zip using Upper and Lowwer Case letters")
        Console.WriteLine("         -e                  Crack zip using letters and numbers")
        Console.WriteLine("         -w [filename]       Crack zip using a dictionary")
        Console.WriteLine("         -d                  Decompress the zip file")
        Console.WriteLine("         -o [directory]      Output directory after decompression")
        Console.WriteLine("         -i                  Get zip info")
        Console.WriteLine("         -y                  Get zip content")
        Console.WriteLine("         -h                  Show help (this)")
    End Sub

    Dim base26 As Char() = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Dim base36 As Char() = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Dim base52 As Char() = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
    Dim base62 As Char() = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

    Function getPass(ByVal n As Long, ByVal mode As Char) As String
        Dim rtn As String = ""
        Dim nn As Integer = 0
        Dim quarry As Integer = 0
        If mode = "A" Then
            While n > 0
                Dim modu As Integer = n Mod 26
                rtn = rtn & base26(modu)
                n = (n - modu) / 26
            End While
            rtn = StrReverse(rtn)
        ElseIf mode = "B" Then
            While n > 0
                Dim modu As Integer = n Mod 26
                rtn = rtn & base26(modu)
                n = (n - modu) / 26
            End While
            rtn = StrReverse(rtn)
            rtn = rtn.ToLower
        ElseIf mode = "C" Then
            While n > 0
                Dim modu As Integer = n Mod 36
                rtn = rtn & base36(modu)
                n = (n - modu) / 36
            End While
            rtn = StrReverse(rtn)
        ElseIf mode = "D" Then
            While n > 0
                Dim modu As Integer = n Mod 52
                rtn = rtn & base52(modu)
                n = (n - modu) / 52
            End While
            rtn = StrReverse(rtn)
        ElseIf mode = "E" Then
            While n > 0
                Dim modu As Integer = n Mod 62
                rtn = rtn & base62(modu)
                n = (n - modu) / 62
            End While
            rtn = StrReverse(rtn)
        End If
        Return rtn
    End Function

    Public Class CrackParams
        Public startI As Long
        Public endI As Long
        Public methode As Char
    End Class



End Module
