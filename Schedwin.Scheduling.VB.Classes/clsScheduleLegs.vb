Imports System.Data.SqlClient
Imports Schedwin.Common.OldVB

Public Class clsScheduleLegs
    Implements ICopy

#Region " Variables "


    'SQL Columns.
    Private m_IDX_Leg As Integer
    Private m_IDX_ACPilot As Integer
    Private m_Etd As String
    Private m_FromAP As String
    Private m_IDX_FromAP As Integer
    Private m_FromRefuel As Boolean
    Private m_FromTurnTime As Integer
    Private m_Eta As String
    Private m_ToAP As String
    Private m_IDX_ToAP As Integer
    Private m_ToRefuel As Boolean
    Private m_ToTurnTime As Integer
    Private m_Distance As Integer
    Private m_GameFlight As Integer
    Private m_PaxLoaded As Integer
    Private m_FromMTOW As Double
    Private m_ToMLW As Double
    Private m_AltAP As String
    Private m_IDX_AltAP As Integer
    Private m_DistanceToAlt As Integer

    'Other
    Private m_MTOW As Double
    Private m_MLW As Double
    Private m_TotalAvailWT As Double
    Private m_TotalPaxWT As Double
    Private m_TotalFuelWT As Double
    Private m_LegFuelWT As Double
    Private m_LegFuelWTOther As Double
    Private m_AltAPFuelWT As Double
    Private m_ConFuelWT As Double
    Private m_ScheduledGroup As String
    Private m_HasChanged As Boolean
    Private m_Edit As Boolean
    Private m_IsEmptyLeg As Boolean
    Private m_HasEtdChanged As Boolean
    Private m_ServerName As String
    Private m_DatabaseName As String
    Private m_Username As String
    Private m_Password As String


    'Collection
    Private m_LegRes As New Collection

#End Region

#Region " System Defined Procedures "

    Public Sub New()

        m_IDX_Leg = 0
        m_IDX_ACPilot = 0
        m_Etd = ""
        m_FromAP = ""
        m_IDX_FromAP = 0
        m_FromRefuel = False
        m_FromTurnTime = 0
        m_Eta = ""
        m_ToAP = ""
        m_IDX_ToAP = 0
        m_ToRefuel = False
        m_ToTurnTime = 0
        m_Distance = 0
        m_GameFlight = 0
        m_PaxLoaded = 0
        m_FromMTOW = 0.0
        m_ToMLW = 0.0
        m_MTOW = 0.0
        m_MLW = 0.0
        m_TotalAvailWT = 0.0
        m_TotalPaxWT = 0.0
        m_TotalFuelWT = 0.0
        m_LegFuelWT = 0.0
        m_LegFuelWTOther = 0.0
        m_AltAPFuelWT = 0.0
        m_ConFuelWT = 0.0
        m_AltAP = ""
        m_IDX_AltAP = 0
        m_DistanceToAlt = 0
        m_ScheduledGroup = ""
        m_IsEmptyLeg = True
        m_HasEtdChanged = False
        m_ServerName = ""
        m_DatabaseName = ""
        m_Username = ""
        m_Password = ""
    End Sub

    Public Sub New(ByVal _IDX_Leg As Integer,
                   ByVal _IDX_ACPilot As Integer,
                   ByVal _Etd As String,
                   ByVal _FromAP As String,
                   ByVal _IDX_FromAP As Integer,
                   ByVal _FromRefuel As Boolean,
                   ByVal _FromTurnTime As Integer,
                   ByVal _Eta As String,
                   ByVal _ToAP As String,
                   ByVal _IDX_ToAP As Integer,
                   ByVal _ToRefuel As Boolean,
                   ByVal _ToTurnTime As Integer,
                   ByVal _Distance As Integer,
                   ByVal _GameFlight As Integer,
                   ByVal _PaxLoaded As Integer,
                   ByVal _FromMTOW As Double,
                   ByVal _ToMLW As Double,
                   ByVal _AltAP As String,
                   ByVal _IDXAltAP As Integer,
                   ByVal _DistanceToAlt As Integer,
                   ByVal _Aircraft As String,
                   ByVal _ServerName As String,
                   ByVal _DatabaseName As String,
                   ByVal _Username As String,
                   ByVal _Password As String,
                   ByVal GLO_GroupForScheduleList As List(Of GroupForSchedule))

        m_IDX_Leg = _IDX_Leg
        m_IDX_ACPilot = _IDX_ACPilot
        m_Etd = _Etd
        m_FromAP = _FromAP
        m_IDX_FromAP = _IDX_FromAP
        m_FromRefuel = _FromRefuel
        m_FromTurnTime = _FromTurnTime
        m_Eta = _Eta
        m_ToAP = _ToAP
        m_IDX_ToAP = _IDX_ToAP
        m_ToRefuel = _ToRefuel
        m_ToTurnTime = _ToTurnTime
        m_Distance = _Distance
        m_GameFlight = _GameFlight
        m_PaxLoaded = _PaxLoaded
        m_FromMTOW = _FromMTOW
        m_ToMLW = _ToMLW
        m_AltAP = _AltAP
        m_IDX_AltAP = _IDXAltAP
        m_DistanceToAlt = _DistanceToAlt
        m_ServerName = _ServerName
        m_DatabaseName = _DatabaseName
        m_Username = _Username
        m_Password = _Password
        m_MTOW = 0.0
        m_MLW = 0.0
        m_TotalAvailWT = 0.0
        m_TotalPaxWT = 0.0
        m_TotalFuelWT = 0.0
        m_LegFuelWT = 0.0
        m_LegFuelWTOther = 0.0
        m_AltAPFuelWT = 0.0
        m_ConFuelWT = 0.0
        m_ScheduledGroup = ""
        m_HasChanged = False
        m_Edit = False
        m_HasEtdChanged = False
        m_LegRes = New Collection

        '--Retrieve the scheduled reservations in leg.--

        Dim SQLCmd As String
        Dim SQLds As DataSet
        Dim SQLParam(0) As SqlParameter

        SQLCmd = "[VIEW].[sl_ScheduleLegReservations]"
        SQLParam(0) = New SqlParameter("@IDXLeg", IDX_Leg)
        SQLds = New DataSet

        ExecuteStoredProc(SQLCmd, 0, SQLParam, SQLds, _ServerName, _DatabaseName, _Username, _Password)

        If CheckDataset(SQLds) Then
            LegRes = GetListFromTable(SQLds.Tables(0), New clsScheduleLegRes, LegRes, _ServerName, _DatabaseName, _Username, _Password, GLO_GroupForScheduleList)
        End If

        If LegRes.Count > 0 Then
            m_IsEmptyLeg = False
        End If

        CheckGroupScheduled(_Aircraft, GLO_GroupForScheduleList)

    End Sub

#End Region

#Region " Property "

#Region " Property - SQL Columns "

    Public Property IDX_Leg() As Integer
        Get
            IDX_Leg = m_IDX_Leg
        End Get
        Set(ByVal value As Integer)
            m_IDX_Leg = value
        End Set
    End Property

    Public Property IDX_ACPilot() As Integer
        Get
            IDX_ACPilot = m_IDX_ACPilot
        End Get
        Set(ByVal value As Integer)
            m_IDX_ACPilot = value
        End Set
    End Property

    Public Property Etd() As String
        Get
            Etd = m_Etd
        End Get
        Set(ByVal value As String)
            m_Etd = value
        End Set
    End Property

    Public Property FromAP() As String
        Get
            FromAP = m_FromAP
        End Get
        Set(ByVal value As String)
            m_FromAP = value
        End Set
    End Property

    Public Property IDX_FromAP() As Integer
        Get
            IDX_FromAP = m_IDX_FromAP
        End Get
        Set(ByVal value As Integer)
            m_IDX_FromAP = value
        End Set
    End Property

    Public Property FromRefuel() As Boolean
        Get
            FromRefuel = m_FromRefuel
        End Get
        Set(ByVal value As Boolean)
            m_FromRefuel = value
        End Set
    End Property

    Public Property FromTurnTime() As Double
        Get
            FromTurnTime = Math.Round(m_FromTurnTime, 2)
        End Get
        Set(ByVal value As Double)
            m_FromTurnTime = value
        End Set
    End Property

    Public Property Eta() As String
        Get
            Eta = m_Eta
        End Get
        Set(ByVal value As String)
            m_Eta = value
        End Set
    End Property

    Public Property ToAP() As String
        Get
            ToAP = m_ToAP
        End Get
        Set(ByVal value As String)
            m_ToAP = value
        End Set
    End Property

    Public Property IDX_ToAP() As Integer
        Get
            IDX_ToAP = m_IDX_ToAP
        End Get
        Set(ByVal value As Integer)
            m_IDX_ToAP = value
        End Set
    End Property

    Public Property ToRefuel() As Boolean
        Get
            ToRefuel = m_ToRefuel
        End Get
        Set(ByVal value As Boolean)
            m_ToRefuel = value
        End Set
    End Property

    Public Property ToTurnTime() As Double
        Get
            ToTurnTime = m_ToTurnTime
        End Get
        Set(ByVal value As Double)
            m_ToTurnTime = value
        End Set
    End Property

    Public Property Distance() As Integer
        Get
            Distance = m_Distance
        End Get
        Set(ByVal value As Integer)
            m_Distance = value
        End Set
    End Property

    Public Property GameFlight() As Integer
        Get
            GameFlight = m_GameFlight
        End Get
        Set(ByVal value As Integer)
            m_GameFlight = value
        End Set
    End Property

    Public Property PaxLoaded() As Integer
        Get
            PaxLoaded = m_PaxLoaded
        End Get
        Set(ByVal value As Integer)
            m_PaxLoaded = value
        End Set
    End Property

    Public Property FromMTOW() As Double
        Get
            FromMTOW = Math.Round(m_FromMTOW, 2)
        End Get
        Set(ByVal value As Double)
            m_FromMTOW = value
        End Set
    End Property

    Public Property ToMLW() As Double
        Get
            ToMLW = Math.Round(m_ToMLW, 2)
        End Get
        Set(ByVal value As Double)
            m_ToMLW = value
        End Set
    End Property

    Public Property AltAP() As String
        Get
            AltAP = m_AltAP
        End Get
        Set(ByVal value As String)
            m_AltAP = value
        End Set
    End Property

    Public Property IDX_AltAP() As Integer
        Get
            IDX_AltAP = m_IDX_AltAP
        End Get
        Set(ByVal value As Integer)
            m_IDX_AltAP = value
        End Set
    End Property

    Public Property DistanceToAlt() As Integer
        Get
            DistanceToAlt = m_DistanceToAlt
        End Get
        Set(ByVal value As Integer)
            m_DistanceToAlt = value
        End Set
    End Property

#End Region

#Region " Proprty - Other "

    Public Property MTOW() As Double
        Get
            MTOW = Math.Round(m_MTOW, 2)
        End Get
        Set(ByVal value As Double)
            m_MTOW = value
        End Set
    End Property

    Public Property MLW() As Double
        Get
            MLW = Math.Round(m_MLW, 2)
        End Get
        Set(ByVal value As Double)
            m_MLW = value
        End Set
    End Property

    Public Property TotalAvailWT() As Double
        Get
            TotalAvailWT = Math.Round(m_TotalAvailWT, 2)
        End Get
        Set(ByVal value As Double)
            m_TotalAvailWT = value
        End Set
    End Property

    Public Property TotalPaxWT() As Double
        Get
            TotalPaxWT = Math.Round(m_TotalPaxWT, 2)
        End Get
        Set(ByVal value As Double)
            m_TotalPaxWT = value
        End Set
    End Property

    Public Property TotalFuelWT() As Double
        Get
            TotalFuelWT = Math.Round(m_TotalFuelWT, 2)
        End Get
        Set(ByVal value As Double)
            m_TotalFuelWT = value
        End Set
    End Property

    Public Property LegFuelWT() As Double
        Get
            LegFuelWT = Math.Round(m_LegFuelWT, 2)
        End Get
        Set(ByVal value As Double)
            m_LegFuelWT = value
        End Set
    End Property

    Public Property LegFuelWTOther() As Double
        Get
            LegFuelWTOther = Math.Round(m_LegFuelWTOther, 2)
        End Get
        Set(ByVal value As Double)
            m_LegFuelWTOther = value
        End Set
    End Property

    Public Property AltAPFuelWT() As Double
        Get
            AltAPFuelWT = Math.Round(m_AltAPFuelWT, 2)
        End Get
        Set(ByVal value As Double)
            m_AltAPFuelWT = value
        End Set
    End Property

    Public Property ConFuelWT() As Double
        Get
            ConFuelWT = Math.Round(m_ConFuelWT, 2)
        End Get
        Set(ByVal value As Double)
            m_ConFuelWT = value
        End Set
    End Property

    Public Property ScheduledGroup() As String
        Get
            ScheduledGroup = m_ScheduledGroup
        End Get
        Set(ByVal value As String)
            m_ScheduledGroup = value
        End Set
    End Property

    Public Property HasChanged() As Boolean
        Get
            HasChanged = m_HasChanged
        End Get
        Set(ByVal value As Boolean)
            m_HasChanged = value
        End Set
    End Property

    Public Property Edit() As Boolean
        Get
            Edit = m_Edit
        End Get
        Set(ByVal value As Boolean)
            m_Edit = value
        End Set
    End Property

    Public Property IsEmptyLeg() As Boolean
        Get
            IsEmptyLeg = m_IsEmptyLeg
        End Get
        Set(ByVal value As Boolean)
            m_IsEmptyLeg = value
        End Set
    End Property

    Public Property HasEtdChanged() As Boolean
        Get
            HasEtdChanged = m_HasEtdChanged
        End Get
        Set(ByVal value As Boolean)
            m_HasEtdChanged = value
        End Set
    End Property

#End Region

#Region " Property - Collection "

    Public ReadOnly Property LegRes(ByVal i As Integer) As clsScheduleLegRes
        Get
            If i > 0 Then
                LegRes = m_LegRes(i)
            Else
                LegRes = m_LegRes(1)
            End If
        End Get
    End Property

    Public Property LegRes() As Collection
        Get
            LegRes = m_LegRes
        End Get
        Set(ByVal Value As Collection)
            m_LegRes = Value
        End Set
    End Property

#End Region

#Region " Property - Calcualted "

    ReadOnly Property LegFlightTime() As Long
        Get
            If IDX_FromAP > 0 And IDX_ToAP > 0 Then
                LegFlightTime = (DateDiff(DateInterval.Minute, TimeValue(Etd), TimeValue(Eta)))
            Else
                LegFlightTime = 0
            End If
        End Get
    End Property

    Friend ReadOnly Property LegPaxKM() As Double
        Get
            LegPaxKM = Distance * PaxLoaded
        End Get
    End Property

    Friend ReadOnly Property LegSellRate(ByVal ACSellRate As Double) As Double
        Get
            LegSellRate = ACSellRate * Distance
        End Get
    End Property

#End Region

#End Region

#Region " User Defined Procedures "

    Public Function NewCopy(ByVal params() As Object, ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy Implements ICopy.NewCopy

        Return New clsScheduleLegs(params(0), params(1), CDate(params(2)).ToString("HH:mm"), params(3), params(4),
                                   params(5), params(6), CDate(params(7)).ToString("HH:mm"), params(8), params(9),
                                   params(10), params(11), params(12), params(13), params(14),
                                   params(15), params(16), params(17), params(18), params(19), params(20),
                                   ServerName, DatabaseName, Username, Password, GLO_GroupForScheduleList)

    End Function

    Public Function RowObjects() As Object()

        Return New Object() {IDX_Leg,
                             IDX_ACPilot,
                             IsEmptyLeg,
                             HasEtdChanged,
                             Etd,
                             FromAP,
                             IDX_FromAP,
                             FromRefuel,
                             Eta,
                             ToAP,
                             IDX_ToAP,
                             ToRefuel,
                             ToTurnTime,
                             Distance,
                             GameFlight,
                             AltAP,
                             PaxLoaded,
                             FromMTOW,
                             ToMLW,
                             MTOW,
                             MLW,
                             TotalAvailWT,
                             TotalPaxWT,
                             TotalFuelWT,
                             LegFuelWT,
                             LegFuelWTOther,
                             AltAPFuelWT,
                             ConFuelWT,
                             ScheduledGroup,
                             ""}

    End Function

    Public Sub CalculatePassengerWT(ByVal _PilotWeight As Integer)

        Dim tmp_weight As Double = 0
        Dim tmp_pax As Integer = 0
        Dim tmp_group As String = ""

        'Calculate the total passenger weight.
        'Calculate the total number of passengers.
        'Create a string of the group names.
        For i As Integer = 1 To LegRes.Count

            tmp_weight += LegRes(i).PaxWeight + LegRes(i).LugWeight
            tmp_group &= LegRes(i).Reservation & " ; "
            tmp_pax += LegRes(i).Pax

        Next i

        'Add pilot weight to the total passenger weight.
        TotalPaxWT = tmp_weight + _PilotWeight
        ScheduledGroup = tmp_group
        PaxLoaded = tmp_pax

        If LegRes.Count = 0 Then
            IsEmptyLeg = True
        Else
            IsEmptyLeg = False
        End If

    End Sub

    Public Sub CalculateLegFuelWT(ByVal _FuelFlow As Double, ByVal _ACSpeed As Double)

        Dim tmp_time As Double = 0.0

        'Calculate fuel weight for leg.
        If LegFlightTime > 0 Then
            m_LegFuelWT = _FuelFlow * (LegFlightTime / 60)
        Else
            m_LegFuelWT = 0
        End If

        'Calculate contingency fuel weight for leg.
        m_ConFuelWT = m_LegFuelWT * 0.05

        'Calculate alternate fuel weight for leg.
        If m_ToRefuel = True Then
            tmp_time = GetFlightTime(m_DistanceToAlt, _ACSpeed)

            If tmp_time > 0 Then
                m_AltAPFuelWT = _FuelFlow * (tmp_time / 60)
            Else
                m_AltAPFuelWT = 0
            End If
        Else
            m_AltAPFuelWT = 0
        End If

    End Sub

    Public Sub CheckGroupScheduled(ByVal _Aircraft As String, _GLO_GroupForScheduleList As List(Of GroupForSchedule))

        For i As Integer = 1 To LegRes.Count

            Dim tmp_row As Integer = 0
            Dim tmp_route As New GroupScheduledRoute

            tmp_row = _GLO_GroupForScheduleList.FindIndex(AddressOf LegRes(i).FindIndexGFSByIDX_RL)

            If tmp_row < 0 Then Continue For

            For ii As Integer = 0 To _GLO_GroupForScheduleList(tmp_row).Route.Count - 1

                If _GLO_GroupForScheduleList(tmp_row).Route(ii).ETA = Eta _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).ETD = Etd _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_FromAP = IDX_FromAP _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_ToAP = IDX_ToAP Then

                    _GLO_GroupForScheduleList(tmp_row).Route.RemoveAt(ii)

                    Exit For
                End If

            Next ii

            tmp_route.ETA = Eta
            tmp_route.ETD = Etd
            tmp_route.FromAP = FromAP
            tmp_route.IDX_FromAP = IDX_FromAP
            tmp_route.IDX_ToAP = IDX_ToAP
            tmp_route.ToAP = ToAP
            tmp_route.Aircraft = _Aircraft

            _GLO_GroupForScheduleList(tmp_row).Route.Add(tmp_route)

            _GLO_GroupForScheduleList(tmp_row).Route.Sort(Function(p1 As GroupScheduledRoute, p2 As GroupScheduledRoute) CDate(p1.ETD).CompareTo(CDate(p2.ETD)))

        Next i

    End Sub

    ''' <summary>
    ''' Change the scheduled list information for the groups in this leg.
    ''' </summary>
    ''' <param name="_NewETD">String: The new ETD time in HH:mm format.</param>
    ''' <param name="_NewETA">String: The new ETA time in HH:mm format.</param>
    ''' <remarks></remarks>
    Public Sub ChangeGroupScheduled(ByVal _NewETD As String, ByVal _NewETA As String, ByVal _Aircraft As String, _GLO_GroupForScheduleList As List(Of GroupForSchedule))

        For i As Integer = 1 To LegRes.Count

            Dim tmp_row As Integer = 0
            Dim tmp_route As New GroupScheduledRoute

            tmp_row = _GLO_GroupForScheduleList.FindIndex(AddressOf LegRes(i).FindIndexGFSByIDX_RL)

            If tmp_row < 0 Then Continue For

            For ii As Integer = 0 To _GLO_GroupForScheduleList(tmp_row).Route.Count - 1

                If _GLO_GroupForScheduleList(tmp_row).Route(ii).ETA = Eta _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).ETD = Etd _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_FromAP = IDX_FromAP _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_ToAP = IDX_ToAP Then

                    _GLO_GroupForScheduleList(tmp_row).Route.RemoveAt(ii)

                    Exit For
                End If

            Next ii

            tmp_route.ETA = _NewETA
            tmp_route.ETD = _NewETD
            tmp_route.FromAP = FromAP
            tmp_route.IDX_FromAP = IDX_FromAP
            tmp_route.IDX_ToAP = IDX_ToAP
            tmp_route.ToAP = ToAP
            tmp_route.Aircraft = _Aircraft

            _GLO_GroupForScheduleList(tmp_row).Route.Add(tmp_route)

            _GLO_GroupForScheduleList(tmp_row).Route.Sort(Function(p1 As GroupScheduledRoute, p2 As GroupScheduledRoute) CDate(p1.ETD).CompareTo(CDate(p2.ETD)))

        Next i

    End Sub

    Public Sub DetachGroupScheduled(_GLO_GroupForScheduleList As List(Of GroupForSchedule))

        For i As Integer = 1 To LegRes.Count

            Dim tmp_row As Integer = 0

            tmp_row = _GLO_GroupForScheduleList.FindIndex(AddressOf LegRes(i).FindIndexGFSByIDX_RL)

            If tmp_row < 0 Then Continue For

            For ii As Integer = 0 To _GLO_GroupForScheduleList(tmp_row).Route.Count - 1

                If _GLO_GroupForScheduleList(tmp_row).Route(ii).ETA = Eta _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).ETD = Etd _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_FromAP = IDX_FromAP _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_ToAP = IDX_ToAP Then

                    _GLO_GroupForScheduleList(tmp_row).Route.RemoveAt(ii)

                    Exit For
                End If

            Next ii

            _GLO_GroupForScheduleList(tmp_row).Route.Sort(Function(p1 As GroupScheduledRoute, p2 As GroupScheduledRoute) CDate(p1.ETD).CompareTo(CDate(p2.ETD)))

        Next i

    End Sub

    Public Sub ChangeGroupScheduled(ByVal _NewETD As String, ByVal _NewETA As String, ByVal _IDX_FromAP As Integer, ByVal _FromAP As String,
                                    ByVal _IDX_ToAP As Integer, ByVal _ToAP As String, ByVal _Aircraft As String,
                                    _GLO_GroupForScheduleList As List(Of GroupForSchedule))

        For i As Integer = 1 To LegRes.Count

            Dim tmp_row As Integer = 0
            Dim tmp_route As New GroupScheduledRoute

            tmp_row = _GLO_GroupForScheduleList.FindIndex(AddressOf LegRes(i).FindIndexGFSByIDX_RL)

            If tmp_row < 0 Then Continue For

            For ii As Integer = 0 To _GLO_GroupForScheduleList(tmp_row).Route.Count - 1

                If _GLO_GroupForScheduleList(tmp_row).Route(ii).ETA = Eta _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).ETD = Etd _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_FromAP = IDX_FromAP _
                        And _GLO_GroupForScheduleList(tmp_row).Route(ii).IDX_ToAP = IDX_ToAP Then

                    _GLO_GroupForScheduleList(tmp_row).Route.RemoveAt(ii)

                    Exit For
                End If

            Next ii

            tmp_route.ETA = _NewETA
            tmp_route.ETD = _NewETD
            tmp_route.FromAP = _FromAP
            tmp_route.IDX_FromAP = _IDX_FromAP
            tmp_route.IDX_ToAP = _IDX_ToAP
            tmp_route.ToAP = _ToAP
            tmp_route.Aircraft = _Aircraft

            _GLO_GroupForScheduleList(tmp_row).Route.Add(tmp_route)

            _GLO_GroupForScheduleList(tmp_row).Route.Sort(Function(p1 As GroupScheduledRoute, p2 As GroupScheduledRoute) CDate(p1.ETD).CompareTo(CDate(p2.ETD)))

        Next i

    End Sub

    Public Function IsGroupOnLeg(ByVal _IDX_RL As Integer) As Boolean

        For i As Integer = 1 To LegRes.Count
            If LegRes(i).IDX_RL = _IDX_RL Then Return True
        Next i

        Return False

    End Function

#End Region

End Class
