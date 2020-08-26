Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports ShowMessage

Public Module GlobalUtil

#Region "General methods"


    ''' <summary>
    ''' Get a list of all Pilot Types to be loaded into pilot type column in grid.
    ''' </summary>
    ''' <param name="_List"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadPilotTypeIntoGridColCombo(ByRef _List As String, ServerName As String, DatabaseName As String, Username As String, Password As String) As Boolean

        _List = ""

        Try

            Dim SQLCmd As String
            Dim SQLds As DataSet

            SQLCmd = "SELECT IDX, PilotType FROM vlst_PilotTypes ORDER BY PilotType;"
            SQLds = New DataSet

            ExecuteView(SQLCmd, 10, SQLds, ServerName, DatabaseName, Username, Password)

            If CheckDataset(SQLds) Then

                _List = "0;;|"

                For i As Integer = 0 To SQLds.Tables(0).Rows.Count - 1
                    _List &= CStr(SQLds.Tables(0).Rows(i).Item("IDX")) & ";" & CStr(SQLds.Tables(0).Rows(i).Item("PilotType")) & ";|"
                Next

            End If

            If _List.Length > 0 Then
                _List = _List.Substring(0, _List.Length - 1)
            End If

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.LoadPilotTypeIntoGridColCombo" & vbCrLf & ex.ToString)
            _List = DefaultColumnList
            Return False
        End Try

        Return True

    End Function
    Friend Function CheckCoordinates(ByVal direction As String, ByVal val_option As SCH_Coordinates) As Boolean

        Try
            Select Case val_option
                Case SCH_Coordinates.Longitude

                    Dim tmp_RegExp As New Regex("^([EW]([0][0-9][0-9]|[1]([0-7][0-9]|[8][0]))).(([0-5][0-9])|([6][0])).([0-9][0-9])$")

                    If Not tmp_RegExp.IsMatch(direction) Then
                        Return False
                    End If

                Case SCH_Coordinates.Latitude

                    Dim tmp_RegExp As New Regex("^([NS]([0-8][0-9]|[9][0])).(([0-5][0-9])|([6][0])).([0-9][0-9])$")

                    If Not tmp_RegExp.IsMatch(direction) Then
                        Return False
                    End If

            End Select

        Catch ex As Exception
            Logs.LogError("CheckCoordinates " & vbCrLf & ex.ToString)
            Throw ex
        End Try

        Return True

    End Function


    ''' <summary>
    ''' Return the distance between two air strips.
    ''' </summary>
    ''' <param name="airport1">Long: ID of the from airport.</param>
    ''' <param name="airport2">Long: ID of the to airport.</param>
    ''' <param name="calculateDist">Boolean: If true then the distance will be calculated from the coordinates of airstrips. If false then the distance is what is saved in database.</param>
    ''' <returns>Integer: Distance value between the two airports.</returns>
    ''' <remarks></remarks>
    Function GetDistance(ByVal airport1 As Long, ByVal airport2 As Long,
                         ServerName As String, DatabaseName As String, Username As String, Password As String,
                         Optional ByVal calculateDist As Boolean = False) As Integer

        Try

            Dim tmp_ds As Data.DataSet
            Dim tmp_obj As Object = ""
            Dim SQLAns As Long = 0
            Dim SQLCmd As String = ""

            If airport1 = airport2 Then
                Return 0
            End If 'Check if airprots the same.

            If calculateDist = True Then
                GoTo CALCDISTANCE
            End If 'Must the distance be calcualted.

            tmp_ds = New DataSet
            tmp_ds = da.getRecordSet("SELECT Distinct Distance FROM vg_Distance WHERE (StartAP = " & airport1 & " AND DestAP = " & airport2 & ") OR (StartAP = " & airport2 & " AND DestAP = " & airport1 & ");", 10)

            If CheckDataset(tmp_ds) Then
                GetDistance = tmp_ds.Tables(0).Rows(0).Item("Distance")
            Else
CALCDISTANCE:

                '--calculate the distance.--

                Dim tmp_lat1 As String = ""
                Dim tmp_lon1 As String = ""
                Dim tmp_lat2 As String = ""
                Dim tmp_lon2 As String = ""
                Dim tmp_iata1 As String = ""
                Dim tmp_iata2 As String = ""

                tmp_ds = New Data.DataSet
                tmp_ds = da.getRecordSet("SELECT DISTINCT Latitude, Longitude, IATA FROM vg_Airports WHERE IDX = " & airport1 & " OR IDX = " & airport2 & ";", 10)

                If CheckDataset(tmp_ds) Then
                    For i As Integer = 0 To tmp_ds.Tables(0).Rows.Count - 1

                        If i = 0 Then

                            tmp_lat1 = tmp_ds.Tables(0).Rows(i).Item("Latitude")
                            tmp_lon1 = tmp_ds.Tables(0).Rows(i).Item("Longitude")
                            tmp_iata1 = tmp_ds.Tables(0).Rows(i).Item("IATA")

                            If Not CheckCoordinates(tmp_lat1, SCH_Coordinates.Latitude) Or Not CheckCoordinates(tmp_lon1, SCH_Coordinates.Longitude) Then
                                ShowMsg("Check the longitude and latitude information for " & tmp_iata1, ShowMsgImage.Alert)
                                tmp_lat1 = ""
                                tmp_lon1 = ""
                            End If 'check if coordinates valid.

                        Else

                            tmp_lat2 = tmp_ds.Tables(0).Rows(i).Item("Latitude")
                            tmp_lon2 = tmp_ds.Tables(0).Rows(i).Item("Longitude")
                            tmp_iata2 = tmp_ds.Tables(0).Rows(i).Item("IATA")

                            If Not CheckCoordinates(tmp_lat2, SCH_Coordinates.Latitude) Or Not CheckCoordinates(tmp_lon2, SCH_Coordinates.Longitude) Then
                                ShowMsg("Check the longitude and latitude information for " & tmp_iata2, ShowMsgImage.Alert)
                                tmp_lat2 = ""
                                tmp_lon2 = ""
                            End If 'check if coordinates valid.

                        End If 'assign different coordinates to the airstrips.
                    Next i 'loop through airstrips.
                End If 'were there coordinate information for airstrips.

                If tmp_lat1 = "" Or tmp_lat2 = "" Or tmp_lon2 = "" Or tmp_lon1 = "" Then

                    GetDistance = 0

                    While Not IsNumeric(tmp_obj)
                        tmp_obj = InputBox("Please supply a distance between these two airstrips " & tmp_iata1 & " and " & tmp_iata2 & ".", "Save Distance", GetDistance)
                    End While

                    SQLCmd = "[SAVING].[ss_AirportDistance]"
                    SQL_03Params(0) = New SqlParameter("@IDXStartAP", airport1)
                    SQL_03Params(1) = New SqlParameter("@IDXDestAP", airport2)
                    SQL_03Params(2) = New SqlParameter("@Distance", tmp_obj)
                    SQLAns = 0

                    ExecuteStoredProcedure(SQLCmd, 10, SQL_03Params, SQLAns, ServerName, DatabaseName, Username, Password)

                    If SQLAns <> 0 Then
                        ShowMsg("Could not save distance.", ShowMsgImage.Alert)
                        Return 0
                    End If

                    GetDistance = CInt(tmp_obj)

                Else

                    GetDistance = CalculateDistance(ConvertLatLon(tmp_lat1), ConvertLatLon(tmp_lon1), ConvertLatLon(tmp_lat2), ConvertLatLon(tmp_lon2))

                    While Not IsNumeric(tmp_obj)
                        tmp_obj = InputBox("Please supply a distance value between the two airstrips " & tmp_iata1 & " and " & tmp_iata2 & ".", "Save Distance", GetDistance)
                    End While

                    SQLCmd = "[SAVING].[ss_AirportDistance]"
                    SQL_03Params(0) = New SqlParameter("@IDXStartAP", airport1)
                    SQL_03Params(1) = New SqlParameter("@IDXDestAP", airport2)
                    SQL_03Params(2) = New SqlParameter("@Distance", tmp_obj)
                    SQLAns = 0

                    ExecuteStoredProcedure(SQLCmd, 10, SQL_03Params, SQLAns, ServerName, DatabaseName, Username, Password)

                    If SQLAns <> 0 Then
                        ShowMsg("Could not save distance.", ShowMsgImage.Alert)
                        Return 0
                    End If

                    GetDistance = CInt(tmp_obj)

                End If 'check airstrip coordinates correct.

            End If 'check if distance exist in table.

        Catch ex As Exception
            ShowMsg(ex, ShowMsgImage.Critical)
            Logs.LogError("programs.GetDistance " & vbCrLf & ex.ToString)
            Return 0
        End Try

        Return GetDistance

    End Function

    ''' <summary>
    ''' Receivies radian coordinates and calculates the distance between the two radian points.
    ''' </summary>
    ''' <param name="lat1">Radian value of Departure Latitude.</param>
    ''' <param name="lon1">Radian value of Deprature Longitude.</param>
    ''' <param name="lat2">Radian value of Destination Latitude.</param>
    ''' <param name="lon2">Radian value of Destination Longitude.</param>
    ''' <returns>Distance between two radian coordinates.</returns>
    ''' <remarks></remarks>
    Friend Function CalculateDistance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double) As Double

        Try

            Dim tmp_dist As Double

            tmp_dist = (System.Math.Sin(lat1) * System.Math.Sin(lat2)) + (System.Math.Cos(lat1) * System.Math.Cos(lat2) * System.Math.Cos(lon2 - lon1))
            CalculateDistance = 6378.7 * Math.Acos(tmp_dist)

        Catch ex As Exception
            MsgBox("Error:" & vbCrLf & ex.ToString)
            Logs.LogError("CalculateDistance " & vbCrLf & ex.ToString)
            Return 0
        End Try

    End Function

    ''' <summary>
    ''' Converts coordinates into a radian value.
    ''' </summary>
    ''' <param name="grad">String: Coordinate value to change.</param>
    ''' <returns>Double: Returns a radian value of the coordinates.</returns>
    ''' <remarks></remarks>
    Friend Function ConvertLatLon(ByVal grad As String) As Double

        Dim deg As Double
        Dim min As Double
        Dim sec As Double

        If Left(grad, 1) = "E" Or Left(grad, 1) = "W" Then

            If Not CheckCoordinates(grad, SCH_Coordinates.Longitude) Then Return 0

            deg = Mid(grad, 2, 3)
            min = Mid(grad, 6, 2)
            sec = Mid(grad, 9, 2)
        ElseIf Left(grad, 1) = "N" Or Left(grad, 1) = "S" Then

            If Not CheckCoordinates(grad, SCH_Coordinates.Latitude) Then Return 0

            deg = Mid(grad, 2, 2)
            min = Mid(grad, 5, 2)
            sec = Mid(grad, 8, 2)
        End If

        ConvertLatLon = (deg + min / 60 + sec / 3600) * Math.PI / 180

    End Function

    ''' <summary>
    ''' Checks the oject value and length and if numeric and returns true if it has a value.
    ''' </summary>
    ''' <param name="Obj">Object: Any object Type</param>
    ''' <returns>Boolean: True or False</returns>
    ''' <remarks></remarks>
    Function CheckObjectNumericValue(ByVal Obj As Object) As Boolean

        If Not Obj Is DBNull.Value Then
            If Not Obj Is Nothing Then
                If Obj.ToString.Length > 0 Then
                    If IsNumeric(Obj) Then
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If 'Check if object length greater then 0.
            Else
                Return False
            End If 'Check if object is nothing.
        Else
            Return False
        End If 'Check if object is null.

        Return True

    End Function

    ''' <summary>
    ''' Round to nearest whole five value.
    ''' </summary>
    ''' <param name="num">Double: Number to be checked.</param>
    ''' <returns>Double: The rounded number.</returns>
    ''' <remarks></remarks>
    Friend Function NearestFive(ByVal num As Double) As Double
        Try

            'Dim tmp_dummy1 As Integer
            'Dim tmp_dummy2 As Integer

            'tmp_dummy1 = num Mod 5
            'If tmp_dummy1 > 0 And tmp_dummy1 < 3 Then
            '    tmp_dummy1 = 0
            'ElseIf tmp_dummy1 < 5 And tmp_dummy1 > 2 Then
            '    tmp_dummy1 = 5
            'End If

            'tmp_dummy2 = Int(num / 10)
            'tmp_dummy2 = tmp_dummy2 * 10

            'NearestFive = tmp_dummy2 + tmp_dummy1

            Dim tmp As Double

            tmp = num / 5
            tmp = Math.Round(tmp, 0)

            NearestFive = tmp * 5

        Catch ex As Exception
            Logs.LogError("NearestFive " & vbCrLf & ex.ToString)
            Return num
        End Try

    End Function

    ''' <summary>
    ''' Round ETA/ETD time to nearest 5.
    ''' </summary>
    ''' <param name="etd">Use ETD time.</param>
    ''' <param name="eta">Use ETA time.</param>
    ''' <param name="time">Time to check.</param>
    ''' <returns>Rounded time value.</returns>
    ''' <remarks></remarks>
    Function RoundETA_ETD(ByVal etd As Boolean, ByVal eta As Boolean, ByVal time As Date) As Date
        Try
            RoundETA_ETD = time

            If etd Then
                Dim tmp_Minutes As Integer = time.Minute
                Dim tmp_MODMinutes As Integer = tmp_Minutes Mod 5

                If tmp_MODMinutes > 0 Then
                    RoundETA_ETD = time.AddMinutes(NearestFive(tmp_MODMinutes) - tmp_MODMinutes)
                End If
            ElseIf eta Then
                Dim tmp_Minutes As Integer = time.Minute
                Dim tmp_MODMinutes As Integer = tmp_Minutes Mod 5

                If tmp_MODMinutes > 0 Then
                    RoundETA_ETD = time.AddMinutes(NearestFive(tmp_MODMinutes) - tmp_MODMinutes)
                End If
            End If


        Catch ex As Exception
            ShowMsg(ex, ShowMsgImage.Critical)
            Logs.LogError("modSchedule.RoundETA_ETD" & vbCrLf & ex.ToString)
            Return time
        End Try

        Return RoundETA_ETD

    End Function

    Public Function GetListFromTable(ByVal Table As DataTable, ByVal DummyObj As ICopy, ByVal ObjList As IList,
                                     ServerName As String, DatabaseName As String, Username As String, Password As String,
                                     GLO_GroupForScheduleList As List(Of GroupForSchedule)) As IList
        For Each dr As DataRow In Table.Rows
            ObjList.Add(DummyObj.NewCopy(dr.ItemArray, ServerName, DatabaseName, Username, Password, GLO_GroupForScheduleList))
        Next
        Return ObjList
    End Function

    Public Function GetFlightTime(ByVal _Distance As Single, ByVal _ACSpeed As Single) As Double

        If _ACSpeed > 0 Then
            GetFlightTime = ((_Distance / _ACSpeed) * 60)
        Else
            GetFlightTime = 0
        End If

    End Function

    Public Function CheckObject(ByVal Obj As Object) As Boolean

        If Not Obj Is DBNull.Value Then
            If Not Obj Is Nothing Then
            Else
                Return False
            End If 'Check if object is nothing.
        Else
            Return False
        End If 'Check if object is null.

        Return True

    End Function


    Public Function CheckObjectStringValue(ByVal Obj As Object) As Boolean

        If Not Obj Is DBNull.Value Then
            If Not Obj Is Nothing Then
                If Obj.ToString.Length > 0 Then
                Else
                    Return False
                End If 'Check if object length greater then 0.
            Else
                Return False
            End If 'Check if object is nothing.
        Else
            Return False
        End If 'Check if object is null.

        Return True

    End Function

    ''' <summary>
    ''' Checks the dataset that was referenced for validity. If there are any problems with it 
    ''' the finction returns a false value.
    ''' </summary>
    ''' <param name="dataset">Data.Dataset: Dateset being checked.</param>
    ''' <returns>Boolean: False if there is something wrong with dataset.</returns>
    ''' <remarks></remarks>
    Public Function CheckDataset(ByVal dataset As DataSet) As Boolean

        If Not dataset Is Nothing Then
            If Not dataset Is DBNull.Value Then
                If dataset.Tables.Count > 0 Then
                    For i As Integer = 0 To dataset.Tables.Count - 1
                        If dataset.Tables(i).Rows.Count > 0 Then
                        Else
                            Return False
                        End If 'Check if dataset table has rows.
                    Next i
                Else
                    Return False
                End If 'Check if dataset has tables.
            Else
                Return False
            End If 'Check if dataset is Null.
        Else
            Return False
        End If 'Check if dataset is Nothing.

        Return True

    End Function
#End Region

#Region " Setup IMCFlexGrid Column "

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Public Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal col As Integer,
                              ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                              ByVal cVisible As Boolean, ByVal cLabel As String, ByVal cEdit As Boolean, Optional ByVal cWidth As Integer = 100)

        _Grid.Grid(0, col) = cLabel
        _Grid.Grid.Cols(col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(col).TextAlign = cAlign
        _Grid.Grid.Cols(col).Visible = cVisible
        _Grid.Grid.Cols(col).Width = cWidth
        _Grid.Grid.Cols(col).AllowEditing = cEdit

    End Sub

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Public Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal col As Integer,
                              ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                              ByVal cVisible As Boolean, ByVal cLabel As String,
                              ByVal cEdit As Boolean, ByVal cWidth As Integer,
                              ByVal _cType As String)

        _Grid.Grid(0, col) = cLabel
        _Grid.Grid.Cols(col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(col).TextAlign = cAlign
        _Grid.Grid.Cols(col).Visible = cVisible
        _Grid.Grid.Cols(col).Width = cWidth
        _Grid.Grid.Cols(col).AllowEditing = cEdit
        Select Case _cType
            Case "String"
                _Grid.Grid.Cols(col).DataType = GetType(String)
            Case "Integer"
                _Grid.Grid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                _Grid.Grid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                _Grid.Grid.Cols(col).DataType = GetType(Double)
            Case "Date"
                _Grid.Grid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                _Grid.Grid.Cols(col).DataType = GetType(DateTime)
        End Select

    End Sub

    ''' <summary>
    ''' Thr grid column values that you nees to ser.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <param name="_cColCombo">String: The list for the column combo.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal col As Integer,
                          ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                          ByVal cVisible As Boolean, ByVal cLabel As String,
                          ByVal cEdit As Boolean, ByVal cWidth As Integer,
                          ByVal _cType As String, ByVal _cColCombo As String)

        _Grid.Grid(0, col) = cLabel
        _Grid.Grid.Cols(col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(col).TextAlign = cAlign
        _Grid.Grid.Cols(col).Visible = cVisible
        _Grid.Grid.Cols(col).Width = cWidth
        _Grid.Grid.Cols(col).AllowEditing = cEdit
        Select Case _cType
            Case "String"
                _Grid.Grid.Cols(col).DataType = GetType(String)
            Case "Integer"
                _Grid.Grid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                _Grid.Grid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                _Grid.Grid.Cols(col).DataType = GetType(Double)
            Case "Date"
                _Grid.Grid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                _Grid.Grid.Cols(col).DataType = GetType(DateTime)
        End Select
        _Grid.ColCombo(col) = _cColCombo

    End Sub

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal col As Integer,
                              ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                              ByVal cVisible As Boolean, ByVal cLabel As String,
                              ByVal cEdit As Boolean, ByVal cWidth As Integer,
                              ByVal _cType As String, ByVal _cCellColor As System.Drawing.Color)

        _Grid.Grid(0, col) = cLabel
        _Grid.Grid.Cols(col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(col).TextAlign = cAlign
        _Grid.Grid.Cols(col).Visible = cVisible
        _Grid.Grid.Cols(col).Width = cWidth
        _Grid.Grid.Cols(col).AllowEditing = cEdit
        _Grid.Grid.Cols(col).Style.BackColor = _cCellColor

        Select Case _cType
            Case "String"
                _Grid.Grid.Cols(col).DataType = GetType(String)
            Case "Integer"
                _Grid.Grid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                _Grid.Grid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                _Grid.Grid.Cols(col).DataType = GetType(Double)
            Case "Date"
                _Grid.Grid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                _Grid.Grid.Cols(col).DataType = GetType(DateTime)
        End Select

    End Sub

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal col As Integer,
                              ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                              ByVal cVisible As Boolean, ByVal cLabel As String,
                              ByVal cEdit As Boolean, ByVal cWidth As Integer,
                              ByVal _cType As String, ByVal _cCellColor As System.Drawing.Color,
                              ByVal _cFontColor As System.Drawing.Color)

        _Grid.Grid(0, col) = cLabel
        _Grid.Grid.Cols(col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(col).TextAlign = cAlign
        _Grid.Grid.Cols(col).Visible = cVisible
        _Grid.Grid.Cols(col).Width = cWidth
        _Grid.Grid.Cols(col).AllowEditing = cEdit
        _Grid.Grid.Cols(col).Style.BackColor = _cCellColor
        _Grid.Grid.Cols(col).Style.ForeColor = _cFontColor

        Select Case _cType
            Case "String"
                _Grid.Grid.Cols(col).DataType = GetType(String)
            Case "Integer"
                _Grid.Grid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                _Grid.Grid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                _Grid.Grid.Cols(col).DataType = GetType(Double)
            Case "Date"
                _Grid.Grid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                _Grid.Grid.Cols(col).DataType = GetType(DateTime)
        End Select

    End Sub

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="_Grid">ctlIMCFlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="_Col">Integer: Index of the column.</param>
    ''' <param name="_Row">Integer: Index of the row.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef _Grid As IMCFlexGrid.ctlIMCFlexGrid, ByVal _Col As Integer,
                              ByVal _Row As Integer,
                              ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                              ByVal cVisible As Boolean, ByVal cLabel As String, ByVal cEdit As Boolean, Optional ByVal cWidth As Integer = 100)

        _Grid.Grid(_Row, _Col) = cLabel
        _Grid.Grid.Cols(_Col).TextAlignFixed = hAlign
        _Grid.Grid.Cols(_Col).TextAlign = cAlign
        _Grid.Grid.Cols(_Col).Visible = cVisible
        _Grid.Grid.Cols(_Col).Width = cWidth
        _Grid.Grid.Cols(_Col).AllowEditing = cEdit

    End Sub

#End Region

#Region " Setup C1FlexGrid Column "

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="FGrid">C1FlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef FGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal col As Integer,
                                ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                                ByVal cVisible As Boolean, ByVal cLabel As String, ByVal cEdit As Boolean, ByVal cWidth As Integer, Optional ByVal _cType As String = "")

        FGrid(0, col) = cLabel
        FGrid.Cols(col).TextAlignFixed = hAlign
        FGrid.Cols(col).TextAlign = cAlign
        FGrid.Cols(col).Visible = cVisible
        FGrid.Cols(col).Width = cWidth
        FGrid.Cols(col).AllowEditing = cEdit

        Select Case _cType
            Case "String"
                FGrid.Cols(col).DataType = GetType(String)
            Case "Integer"
                FGrid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                FGrid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                FGrid.Cols(col).DataType = GetType(Double)
            Case "Date"
                FGrid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                FGrid.Cols(col).DataType = GetType(DateTime)
        End Select

    End Sub

    ''' <summary>
    ''' The grid column values that you need to set.
    ''' </summary>
    ''' <param name="FGrid">C1FlexGrid: Grid to which the column that you want to setup belong.</param>
    ''' <param name="col">Integer: Index of the column.</param>
    ''' <param name="row">Integer: Index of the row.</param>
    ''' <param name="hAlign">TextAlignEnum: Text alignment of the column header.</param>
    ''' <param name="cAlign">TextAlignEnum: Text alignment of the columns.</param>
    ''' <param name="cVisible">Boolean: Is the column visible.</param>
    ''' <param name="cLabel">String: The text value of the header for the column.</param>
    ''' <param name="cEdit">Boolean: Can the column values be changed.</param>
    ''' <param name="cWidth">Integer: The column width.</param>
    ''' <param name="_cType">String: Representing the column type.</param>
    ''' <remarks></remarks>
    Sub InitGridColumn(ByRef FGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal col As Integer, ByVal row As Integer,
                                ByVal hAlign As C1.Win.C1FlexGrid.TextAlignEnum, ByVal cAlign As C1.Win.C1FlexGrid.TextAlignEnum,
                                ByVal cVisible As Boolean, ByVal cLabel As String, ByVal cEdit As Boolean, ByVal cWidth As Integer, Optional ByVal _cType As String = "")

        FGrid(row, col) = cLabel
        FGrid.Cols(col).TextAlignFixed = hAlign
        FGrid.Cols(col).TextAlign = cAlign
        FGrid.Cols(col).Visible = cVisible
        FGrid.Cols(col).Width = cWidth
        FGrid.Cols(col).AllowEditing = cEdit

        Select Case _cType
            Case "String"
                FGrid.Cols(col).DataType = GetType(String)
            Case "Integer"
                FGrid.Cols(col).DataType = GetType(Integer)
            Case "Boolean"
                FGrid.Cols(col).DataType = GetType(Boolean)
            Case "Double"
                FGrid.Cols(col).DataType = GetType(Double)
            Case "Date"
                FGrid.Cols(col).DataType = GetType(Date)
            Case "DateTime"
                FGrid.Cols(col).DataType = GetType(DateTime)
        End Select

    End Sub

#End Region

#Region " Execute Stored Procedures "

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                 ByVal _Trancaction As SqlTransaction, ByVal _Connection As SqlConnection, ByRef _Value As Integer)

        Dim sqlcmd As SqlCommand = Nothing

        Try

            sqlcmd = New SqlCommand(_SQLCommand, _Connection, _Trancaction)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.AddRange(_Parameters)
            sqlcmd.ExecuteNonQuery()

            _Value = 0

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg (ex,ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteStoredProc_1" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = -1

        Finally
        End Try

    End Sub

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Trancaction As SqlTransaction,
                                 ByVal _Connection As SqlConnection, ByRef _Value As Integer)

        Dim sqlcmd As SqlCommand = Nothing

        Try

            sqlcmd = New SqlCommand(_SQLCommand, _Connection, _Trancaction)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.ExecuteNonQuery()

            _Value = 0

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg (ex,ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteStoredProc_2" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = -1

        Finally
        End Try

    End Sub

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                 ByRef _Value As Integer, ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqltran As SqlTransaction = Nothing
        Dim sqlconstr As String = Nothing

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqltran = Nothing
            sqltran = sqlcon.BeginTransaction()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon, sqltran)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.AddRange(_Parameters)
            sqlcmd.ExecuteNonQuery()

            sqltran.Commit()

            _Value = 0

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            If CheckObject(sqltran) Then sqltran.Rollback()

            Logs.LogError("programs.ExecuteStoredProc_3" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = -1

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub




    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                 ByRef _Value As DataSet, ByVal _ServerName As String, ByVal _DatabaseName As String,
                                 ByVal _UserName As String, ByVal _Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqltran As SqlTransaction = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlds As DataSet = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing

        Try
            'sqlconstr = "Data Source=" & _ServerName & ";Initial Catalog=" & _DatabaseName & ";Integrated Security=FALSE; Connect Timeout= " & _TimeOut & ";User Id= " & _UserName & ";Password=" & _Password & ";"
            sqlconstr = "Data Source=" & _ServerName & ";Initial Catalog=" & _DatabaseName & ";Integrated Security=TRUE; Connect Timeout= " & _TimeOut & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqltran = Nothing
            sqltran = sqlcon.BeginTransaction()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon, sqltran)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.AddRange(_Parameters)

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            If CheckObject(sqltran) Then sqltran.Rollback()

            Logs.LogError("programs.ExecuteStoredProc_6" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByRef _Value As DataSet,
                                 ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqltran As SqlTransaction = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlds As DataSet = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqltran = Nothing
            sqltran = sqlcon.BeginTransaction()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon, sqltran)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            If CheckObject(sqltran) Then sqltran.Rollback()

            Logs.LogError("programs.ExecuteStoredProc_7" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                 ByVal _Trancaction As SqlTransaction, ByVal _Connection As SqlConnection, ByRef _Value As DataSet)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing
        Dim sqlds As DataSet = Nothing

        Try

            sqlcmd = New SqlCommand(_SQLCommand, _Connection, _Trancaction)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.AddRange(_Parameters)

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteStoredProc_8" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally

        End Try

    End Sub

    Public Sub ExecuteStoredProc(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Trancaction As SqlTransaction,
                                 ByVal _Connection As SqlConnection, ByRef _Value As DataSet)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing
        Dim sqlds As DataSet = Nothing

        Try

            sqlcmd = New SqlCommand(_SQLCommand, _Connection, _Trancaction)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.StoredProcedure

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteStoredProc_9" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally

        End Try

    End Sub

    '--Non Debug Execute Stored Procedure.--

    Public Sub ExecuteStoredProcedure(ByVal _SQLCommand As String, ByVal _TimeOut As Integer,
                                  ByRef _ReturnValue As Integer, ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqltran As SqlTransaction = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlreturn As SqlParameter

        _ReturnValue = -1

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlreturn = New SqlParameter("RETURN", SqlDbType.Int)
            sqlreturn.Direction = ParameterDirection.ReturnValue
            sqlreturn.Value = -1

            Try

                sqltran = Nothing
                sqltran = sqlcon.BeginTransaction()

                sqlcmd = New SqlCommand(_SQLCommand, sqlcon, sqltran)
                sqlcmd.CommandTimeout = _TimeOut
                sqlcmd.CommandType = CommandType.StoredProcedure
                sqlcmd.Parameters.Add(sqlreturn)
                sqlcmd.ExecuteNonQuery()

                _ReturnValue = CType(sqlreturn.Value, Integer)

                sqltran.Commit()

            Catch exSQLCmd As Exception

                ShowMsg(exSQLCmd.Message.ToString, ShowMsgImage.Alert)
                Logs.LogError("programs.ExecuteStoredProcedure_1" & vbCrLf & exSQLCmd.ToString & vbCrLf & _SQLCommand)

                If CheckObject(sqltran) Then sqltran.Rollback()

                _ReturnValue = CType(sqlreturn.Value, Integer)

            End Try

        Catch exSQLConn As Exception

            ShowMsg(exSQLConn.Message.ToString, ShowMsgImage.Alert)
            Logs.LogError("programs.ExecuteStoredProcedure_1" & vbCrLf & exSQLConn.ToString & vbCrLf & _SQLCommand)

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

    Public Sub ExecuteStoredProcedure(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                      ByRef _ReturnValue As Integer, ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqltran As SqlTransaction = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlreturn As SqlParameter

        _ReturnValue = -1

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlreturn = New SqlParameter("RETURN", SqlDbType.Int)
            sqlreturn.Direction = ParameterDirection.ReturnValue
            sqlreturn.Value = -1

            Try

                sqltran = Nothing
                sqltran = sqlcon.BeginTransaction()

                sqlcmd = New SqlCommand(_SQLCommand, sqlcon, sqltran)
                sqlcmd.CommandTimeout = _TimeOut
                sqlcmd.CommandType = CommandType.StoredProcedure
                sqlcmd.Parameters.AddRange(_Parameters)
                sqlcmd.Parameters.Add(sqlreturn)
                sqlcmd.ExecuteNonQuery()

                _ReturnValue = CType(sqlreturn.Value, Integer)

                sqltran.Commit()

            Catch exSQLCmd As Exception

                ShowMsg(exSQLCmd.Message.ToString, ShowMsgImage.Alert)
                Logs.LogError("programs.ExecuteStoredProcedure_1" & vbCrLf & exSQLCmd.ToString & vbCrLf & _SQLCommand)

                If CheckObject(sqltran) Then sqltran.Rollback()

                _ReturnValue = CType(sqlreturn.Value, Integer)

            End Try

        Catch exSQLConn As Exception

            ShowMsg(exSQLConn.Message.ToString, ShowMsgImage.Alert)
            Logs.LogError("programs.ExecuteStoredProcedure_1" & vbCrLf & exSQLConn.ToString & vbCrLf & _SQLCommand)

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

#End Region ' Execute Stored Procedures

#Region " Execute Script "

    Public Sub ExecuteScript(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByRef _Value As Integer,
                            ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqlconstr As String = Nothing

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.Text

            sqlcmd.ExecuteNonQuery()

            _Value = 0

        Catch ex As Exception

            ShowMsg(ex.Message.ToString, ShowMsgImage.Alert)
            Logs.LogError("programs.ExecuteScript_2" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)

            _Value = -1

        Finally
        End Try

    End Sub

    Public Sub ExecuteScript(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter, ByRef _Value As Integer,
                             ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqlconstr As String = Nothing

        Try

            sqlconstr = "Data Source=" & ServerName & ";Initial Catalog=" & DatabaseName & ";Integrated Security=SSPI; Connect Timeout= " & _TimeOut & ";User Id= " & Username & ";Password=" & Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.Text
            sqlcmd.Parameters.AddRange(_Parameters)

            sqlcmd.ExecuteNonQuery()

            _Value = 0

        Catch ex As Exception

            ShowMsg(ex.Message.ToString, ShowMsgImage.Alert)
            Logs.LogError("programs.ExecuteScript_3" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)

            _Value = -1

        Finally
        End Try

    End Sub

    Public Sub ExecuteScript(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                             ByVal _Trancaction As SqlTransaction, ByVal _Connection As SqlConnection, ByRef _Value As Integer)

        Dim sqlcmd As SqlCommand = Nothing

        Try

            sqlcmd = New SqlCommand(_SQLCommand, _Connection, _Trancaction)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.Text
            sqlcmd.Parameters.AddRange(_Parameters)

            sqlcmd.ExecuteNonQuery()

            _Value = 0

        Catch ex As Exception

            ShowMsg(ex.Message.ToString, ShowMsgImage.Alert)
            Logs.LogError("programs.ExecuteScript_1" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)

            _Value = -1

        Finally
        End Try

    End Sub

#End Region ' Execute Script

#Region " Execute Views "

    Public Sub ExecuteView(ByVal _SQLCommand As String, ByVal _TimeOut As Integer,
                                 ByRef _Value As DataSet, ByVal _ServerName As String, ByVal _DatabaseName As String,
                                 ByVal _UserName As String, ByVal _Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlds As DataSet = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing

        Try
            'sqlconstr = "Data Source=" & _ServerName & ";Initial Catalog=" & _DatabaseName & ";Integrated Security=true; Connect Timeout= " & _TimeOut & ";User Id= " & _UserName & ";Password=" & _Password & ";"
            sqlconstr = "Data Source=" & _ServerName & ";Initial Catalog=" & _DatabaseName & ";Integrated Security=true; Connect Timeout= " & _TimeOut & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.Text

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteView_2" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

    Public Sub ExecuteView(ByVal _SQLCommand As String, ByVal _TimeOut As Integer, ByVal _Parameters() As SqlParameter,
                                 ByRef _Value As DataSet, ByVal _ServerName As String, ByVal _DatabaseName As String,
                                 ByVal _UserName As String, ByVal _Password As String)

        Dim sqlcmd As SqlCommand = Nothing
        Dim sqlcon As SqlConnection = Nothing
        Dim sqlconstr As String = Nothing
        Dim sqlds As DataSet = Nothing
        Dim sqladapter As SqlDataAdapter = Nothing

        Try

            sqlconstr = "Data Source=" & _ServerName & ";Initial Catalog=" & _DatabaseName & ";Integrated Security=FALSE; Connect Timeout= " & _TimeOut & ";User Id= " & _UserName & ";Password=" & _Password & ";"
            sqlcon = New SqlConnection(sqlconstr)
            sqlcon.Open()

            sqlcmd = New SqlCommand(_SQLCommand, sqlcon)
            sqlcmd.CommandTimeout = _TimeOut
            sqlcmd.CommandType = CommandType.Text
            sqlcmd.Parameters.AddRange(_Parameters)

            sqladapter = New SqlDataAdapter(sqlcmd)
            sqlds = New DataSet
            sqladapter.Fill(sqlds)

            _Value = sqlds

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString & vbCrLf & _SQLCommand, ShowMsgImage.Critical)
#Else
            ShowMsg(ex, ShowMsgImage.Critical)
#End If

            Logs.LogError("programs.ExecuteView_2" & vbCrLf & ex.ToString & vbCrLf & _SQLCommand)
            _Value = Nothing

        Finally
            If CheckObject(sqlcon) Then
                If sqlcon.State = ConnectionState.Open Then sqlcon.Close()
            End If
        End Try

    End Sub

#End Region ' Execute Views
End Module
