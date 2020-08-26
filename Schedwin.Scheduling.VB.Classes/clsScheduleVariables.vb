Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports ShowMessage
Imports Schedwin.Common.OldVB
Imports Schedwin.Data.Classes

Public Class clsScheduleVariables

#Region " Variables "



    Public VAR_OpenDate As Date = CDate("01/Jan/1900")
    Public VAR_Group As GroupToBeScheduled
    Public VAR_GroupSelectedList As List(Of GroupToBeScheduled)
    Public VAR_SearchIDXResLeg As Integer = 0
    Public VAR_Status As ScheduleStatus '1:Add|2:Edit|3:View

    Public VAR_GroupsAllShow As Boolean = True
    Public VAR_GroupsNotScheduledShow As Boolean = False
    Public VAR_GroupsDepartureShow As Boolean = False
    Public VAR_GroupsDestinationShow As Boolean = False
    Public VAR_GroupsFaultShow As Boolean = False

    Public Enum ScheduleStatus
        Add
        Edit
        View
    End Enum

    'Reservations Grid Columns
    Public VAR_GRD_CGroupSelected As Integer = 0
    Public VAR_GRD_CGroupFlag As Integer = 1
    Public VAR_GRD_CGroupFrom As Integer = 2
    Public VAR_GRD_CGroupIDX_FromAp As Integer = 3
    Public VAR_GRD_CGroupTo As Integer = 4
    Public VAR_GRD_CGroupIDX_ToAp As Integer = 5
    Public VAR_GRD_CGroupNumPax As Integer = 6
    Public VAR_GRD_CGroupResName As Integer = 7
    Public VAR_GRD_CGroupEx As Integer = 8
    Public VAR_GRD_CGroupFor As Integer = 9
    Public VAR_GRD_CGroupOperator As Integer = 10
    Public VAR_GRD_CGroupIDX_RH As Integer = 11
    Public VAR_GRD_CGroupIDX_RL As Integer = 12
    Public VAR_GRD_CGroupPassengerWeight As Integer = 13
    Public VAR_GRD_CGroupLuggageWeight As Integer = 14
    Public VAR_GRD_CGroupLastDest As Integer = 15
    Public VAR_GRD_CGroupSoleUse As Integer = 16
    Public VAR_GRD_CGroupACType As Integer = 17
    Public VAR_GRD_CGroupIDX_ACType As Integer = 18
    Public VAR_GRD_CGroupCancelled As Integer = 19
    Public VAR_GRD_CGroupEarlyEx As Integer = 20
    Public VAR_GRD_CGroupLatestEx As Integer = 21
    Public VAR_GRD_CGroupEarlyFor As Integer = 22
    Public VAR_GRD_CGroupLatestFor As Integer = 23
    Public VAR_GRD_CGroupGameFlight As Integer = 24
    Public VAR_GRD_CGroupResType As Integer = 25
    Public VAR_GRD_CGroupNotes As Integer = 26
    Public VAR_GRD_CGroupMaxCols As Integer = 27

#End Region

#Region " User Defined Procedures "

    ''' <summary>
    ''' Find the Index of the IDX_RL in the groups to be scheduled list.
    ''' </summary>
    ''' <param name="Group"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindIndexGTBSByIDXResLeg(ByVal Group As GroupToBeScheduled) As Boolean
        Return Group.IDX_RL = VAR_SearchIDXResLeg
    End Function

    ''' <summary>
    ''' Load datasets that will be used to load grid information.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadDatasets(ServerName As String, DatabaseName As String, Username As String, Password As String)

        Dim SQLCmd As String = ""
        Dim SQLParamPilot(0) As SqlParameter
        Dim SQLParamAircraft(0) As SqlParameter
        Dim SQLParamAirport(0) As SqlParameter

        Try

            '--Aircraft.--
            AircraftInfo.LoadAircraftList(ServerName, DatabaseName)
            AircraftType.LoadACTypes(ServerName, DatabaseName)
            '--Airport Limitations.--
            ACAirportLimits.LoadACAirportLimits(ServerName, DatabaseName)
            '--Airport Fuel.--
            AirportFuel.LoadFuelList(ServerName, DatabaseName)
            '--Pilots.--
            PilotInfo.LoadPilotInfo(ServerName, DatabaseName)
            PilotType.LoadPilotTypes(ServerName, DatabaseName)
            AirportClosestFuel.LoadClosestFuelList(VAR_OpenDate, ServerName, DatabaseName)
            '--Airport ReFuel.--


        Catch ex As Exception

#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex,ShowMsgImage.Critical)
#End If

            Logs.LogError("clsScheduleVariables.LoadDatasets" & vbCrLf & ex.ToString)



        End Try

    End Sub

    ''' <summary>
    ''' Check if the group is not violating time window.
    ''' </summary>
    ''' <param name="_ETD">String: The departure time of leg.</param>
    ''' <param name="_ETA">String: The arrival time of leg.</param>
    ''' <param name="_EarlyEx">String: Earliest Ex of the group.</param>
    ''' <param name="_LatestEx">String: Latest Ex of the group.</param>
    ''' <param name="_EarlyFor">String: Earliest For of the group.</param>
    ''' <param name="_LatestFor">String: Latest For of the group.</param>
    ''' <param name="_GroupName">String: Group name being checked.</param>
    ''' <returns>Boolean: True if group in time window. False if there is a time violation.</returns>
    ''' <remarks></remarks>
    Public Function IsGroupInTimeWindow(ByVal _ETD As String, ByVal _ETA As String,
                                        ByVal _EarlyEx As String, ByVal _LatestEx As String,
                                        ByVal _EarlyFor As String, ByVal _LatestFor As String, ByVal _GroupName As String) As Boolean

        Dim tmp_ETD As DateTime = CDate(_ETD)
        Dim tmp_ETA As DateTime = CDate(_ETA)
        Dim tmp_EarlyEx As DateTime = CDate(_EarlyEx)
        Dim tmp_LatestFor As DateTime = CDate(_LatestFor)
        Dim tmp_EarlyFor As DateTime = CDate(_EarlyFor)
        Dim tmp_LatestEx As DateTime = CDate(_LatestEx)

        If tmp_ETD < tmp_EarlyEx Then
            ShowMsg("The ETD:" & tmp_ETD.ToString("HH:mm") & " is earlier than the Earliest Ex:" & tmp_EarlyEx.ToString("HH:mm") & " for " & _GroupName & ". Cannot add group.", ShowMsgImage.Critical)
            Return False
        ElseIf tmp_ETA > tmp_LatestFor Then
            ShowMsg("The ETA:" & tmp_ETA.ToString("HH:mm") & " is later than the Latest For:" & tmp_LatestFor.ToString("HH:mm") & " for " & _GroupName & ". Cannot add group.", ShowMsgImage.Critical)
            Return False
        End If

        Return True

    End Function

#End Region

#Region " Loading Grid ColCombo "

    ''' <summary>
    ''' Create a list of available pilots.
    ''' </summary>
    ''' <returns>String: List of pilots.</returns>
    ''' <remarks></remarks>
    Public Function LoadPilotIntoGridColCombo(ByRef _List As String) As Boolean

        Try
            _List = ""

            Dim tmpList As List(Of PilotInfo) = PilotInfo.GetPilotList()

            If (tmpList IsNot Nothing) Then

                For Each pilotInfo As PilotInfo In tmpList

                    _List &= CStr(pilotInfo.IDX_Personnel) & ";" & pilotInfo.Name & ";|"
                Next

                If _List.Length > 0 Then
                    _List = _List.Substring(0, _List.Length - 1)
                End If
            Else
                _List = DefaultColumnList
            End If


        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex,ShowMsgImage.Critical)
#End If

            _List = DefaultColumnList
            Logs.LogError("clsScheduleVariables.LoadPilotIntoGridColCombo" & vbCrLf & ex.ToString)
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Create a lits of available pilot types.
    ''' </summary>
    ''' <returns>String: List of pilot types.</returns>
    ''' <remarks></remarks>
    Public Function LoadPilotTypeIntoGridColCombo(ByRef _List As String) As Boolean

        Try
            Dim lstItems As List(Of PilotType) = PilotType.GetPilotTypes()

            If (lstItems IsNot Nothing) Then

                _List = ""

                For Each pilotType As PilotType In lstItems

                    _List &= CStr(pilotType.IDX) & ";" & pilotType.Description & ";|"
                Next

                If _List.Length > 0 Then
                    _List = _List.Substring(0, _List.Length - 1)
                End If

            Else
                _List = DefaultColumnList
            End If

        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex,ShowMsgImage.Critical)
#End If

            _List = DefaultColumnList
            Logs.LogError("clsScheduleVariables.LoadPilotTypeIntoGridColCombo" & vbCrLf & ex.ToString)
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Create a list of avaialble aircraft.
    ''' </summary>
    ''' <returns>String: List of aircraft.</returns>
    ''' <remarks></remarks>
    Public Function LoadAircraftIntoGridColCombo(ByRef _List As String) As Boolean

        Try

            Dim lstItems As List(Of AircraftInfo)

            lstItems = AircraftInfo.GetAircraftList(True)

            If (lstItems IsNot Nothing) Then

                For Each aircraftInfo As AircraftInfo In lstItems

                    Dim acType = AircraftType.GetACType(aircraftInfo.IDX_AC_Type)
                    If (acType IsNot Nothing) Then
                        _List &= aircraftInfo.IDX & ";" & aircraftInfo.Registration & ";" & acType.TypeName & "|"
                    End If
                Next


                If _List.Length > 0 Then
                    _List = _List.Substring(0, _List.Length - 1)
                End If

            End If



        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex,ShowMsgImage.Critical)
#End If

            _List = DefaultColumnList
            Logs.LogError("clsScheduleVariables.LoadAircraftIntoGridColCombo" & vbCrLf & ex.ToString)
            Return False
        End Try

        Return True

    End Function

    ''' <summary>
    ''' Create a list of available airports.
    ''' </summary>
    ''' <returns>String: List of airports.</returns>
    ''' <remarks></remarks>
    Public Function LoadAirportIntoGridColCombo(ByRef _List As String) As Boolean

        Try
            Dim lstAirstrips As List(Of AirstripInfo)

            lstAirstrips = AirstripInfo.GetAirstrips()


            For Each airstripInfo As AirstripInfo In lstAirstrips

                _List &= airstripInfo.IDX & ";" & airstripInfo.Code & "; " & airstripInfo.Description & "|"
            Next

            If _List.Length > 0 Then
                _List = _List.Substring(0, _List.Length - 1)
            End If



        Catch ex As Exception
#If DEBUG = True Then
            ShowMsg(ex.ToString, ShowMsgImage.Critical)
#Else
            ShowMsg(ex,ShowMsgImage.Critical)
#End If

            _List = DefaultColumnList
            Logs.LogError("clsScheduleVariables.LoadAirportIntoGridColCombo" & vbCrLf & ex.ToString)
            Return False
        End Try

        Return True

    End Function


    '    Public Function LoadAirportIntoGridColCombo(ByRef _List As String) As Boolean

    '        Try

    '            If CheckDataset(DS_Airports) Then
    '                For i As Integer = 0 To DS_Airports.Tables(0).Rows.Count - 1
    '                    _List &= DS_Airports.Tables(0).Rows(i).Item("IDX") & ";" & DS_Airports.Tables(0).Rows(i).Item("IATA") & "; " & DS_Airports.Tables(0).Rows(i).Item("Airport") & "|"
    '                Next i

    '                If _List.Length > 0 Then
    '                    _List = _List.Substring(0, _List.Length - 1)
    '                End If

    '            End If

    '        Catch ex As Exception
    '#If DEBUG = True Then
    '            ShowMsg(ex.ToString, ShowMsgImage.Critical)
    '#Else
    '            ShowMsg(ex,ShowMsgImage.Critical)
    '#End If

    '            _List = DefaultColumnList
    '            Logs.LogError("clsScheduleVariables.LoadAirportIntoGridColCombo" & vbCrLf & ex.ToString)
    '            Return False
    '        End Try

    '        Return True

    '    End Function

#End Region

End Class
