Imports ShowMessage
Imports System.Text

Public Module Common

    ''' <summary>
    ''' Check that object values supplied is a number and if it is positive or negative.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="status"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidIntegerNumber(ByVal obj As Object, ByVal status As Numeric_Status) As Boolean

        If Not CheckObjectStringValue(obj) Then Return False

        If Not IsNumeric(obj) Then Return False

        obj = CType(obj, Integer)

        If status = Numeric_Status.Positive Then

            If Not obj > 0 Then Return False

        ElseIf status = Numeric_Status.Positive Then

            If Not obj < 0 Then Return False

        End If

        Return True

    End Function

    ''' <summary>
    ''' Get a list of pilot names to be loaded into a list for a combobox.
    ''' </summary>
    ''' <param name="r_List"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadPilotsCompanyIntoComboBox(ByRef r_List As String, ByVal p_ActivePilotsOnly As Boolean,
                                                  ServerName As String, DatabaseName As String, Username As String, Password As String) As Boolean

        Dim SQL_cmd As String
        Dim SQL_ds As DataSet = New DataSet
        Dim PilotList As StringBuilder = New StringBuilder

        r_List = ""

        Try

            Using SQLEntity As CommonSQL = New CommonSQL(ServerName, DatabaseName, Username, Password, 0)

                If p_ActivePilotsOnly Then
                    SQL_cmd = "SELECT IDX_Personnel, PilotName FROM vPilotsCompanyOnly WHERE Active = 1 ORDER BY PilotName;"
                Else
                    SQL_cmd = "SELECT IDX_Personnel, PilotName FROM vPilotsCompanyOnly ORDER BY PilotName;"
                End If

                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Open)
                SQLEntity.ExecCommand(SQL_cmd, CommandType.Text, SQL_ds)
                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Close)

                PilotList.AppendLine("")

                If CheckDataset(SQL_ds) Then
                    For row As Integer = 0 To SQL_ds.Tables(0).Rows.Count - 1
                        PilotList.Append("-" + SQL_ds.Tables(0).Rows(row).Item("PilotName"))
                    Next row
                End If

                r_List = PilotList.ToString()

            End Using

        Catch ex As Exception
            ShowMsg(ex, ShowMsgImage.Critical)
            r_List = ""

            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Get a list of all Pilots to be loaded into pilot column in grdi.
    ''' </summary>
    ''' <param name="r_List"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadPilotsIntoGridColCombo(ByRef r_List As String, ServerName As String, DatabaseName As String, Username As String, Password As String) As Boolean

        Dim SQL_cmd As String = "SELECT IDX, Firstname, Surname FROM vlst_Pilots ORDER BY Firstname, Surname;"
        Dim SQL_ds As DataSet = New DataSet

        r_List = ""

        Try

            Using SQLEntity As CommonSQL = New CommonSQL(ServerName, DatabaseName, Username, Password, 0)

                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Open)
                SQLEntity.ExecCommand(SQL_cmd, CommandType.Text, SQL_ds)
                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Close)

                If CheckDataset(SQL_ds) Then

                    For i As Integer = 0 To SQL_ds.Tables(0).Rows.Count - 1
                        r_List &= CStr(SQL_ds.Tables(0).Rows(i).Item("IDX")) & ";" & CStr(SQL_ds.Tables(0).Rows(i).Item("Firstname")) & " " & CStr(SQL_ds.Tables(0).Rows(i).Item("Surname")) & ";|"
                    Next
                End If

                If r_List.Length > 0 Then
                    r_List = r_List.Substring(0, r_List.Length - 1)
                End If

            End Using

        Catch ex As Exception
            ShowMsg(ex, ShowMsgImage.Critical)
            r_List = DefaultColCombo

            Return False
        End Try

        Return True

    End Function 'Get a list of all Pilots to be loaded into pilot column in grid.

    ''' <summary>
    ''' Load the Airstrips IATA into the grid.colcombo.
    ''' </summary>
    ''' <returns>String of the airstrips.</returns>
    ''' <remarks></remarks>
    Public Function LoadAirportIATAIntoGridColCombo(ByRef r_List As String, ServerName As String, DatabaseName As String, Username As String, Password As String) As Boolean

        Dim SQL_cmd As String = "SELECT DISTINCT IDX, IATA, Airport FROM vg_Airports ORDER BY IATA;"
        Dim SQL_ds As DataSet = New DataSet

        r_List = ""

        Try

            Using SQLEntity As CommonSQL = New CommonSQL(ServerName, DatabaseName, Username, Password, 0)

                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Open)
                SQLEntity.ExecCommand(SQL_cmd, CommandType.Text, SQL_ds)
                SQLEntity.SQLConnection(CommonSQL.ConnectionState.Close)

                If CheckDataset(SQL_ds) Then
                    For i As Integer = 0 To SQL_ds.Tables(0).Rows.Count - 1
                        r_List = r_List & CStr(SQL_ds.Tables(0).Rows(i).Item("IDX")) & ";" & CStr(SQL_ds.Tables(0).Rows(i).Item("IATA")) & "; " & CStr(SQL_ds.Tables(0).Rows(i).Item("Airport")) & "|"
                    Next i
                End If

                If r_List.Length > 0 Then
                    r_List = r_List.Substring(0, r_List.Length - 1)
                End If

            End Using

        Catch ex As Exception
            ShowMsg(ex, ShowMsgImage.Critical)
            r_List = DefaultColCombo

            Return False
        End Try

        Return True

    End Function 'Load the Airstrips IATA into the grid.colcombo.

End Module
