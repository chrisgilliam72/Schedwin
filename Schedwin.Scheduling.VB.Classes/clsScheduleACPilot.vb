Imports System.Data.SqlClient
Imports Schedwin.Common.OldVB
Imports Schedwin.Data.Classes

Public Class clsScheduleACPilot
    Implements ICopy

#Region " Variables "

    'SQL Columns.
    Private m_IDX_ACPilot As Integer
    Private m_Pilot_1 As String
    Private m_IDX_Pilot_1 As Integer
    Private m_PilotType_1 As String
    Private m_IDX_PilotType_1 As Integer
    Private m_PilotAP_1 As String
    Private m_IDX_PilotAP_1 As Integer
    Private m_PilotWeight_1 As Integer
    Private m_PilotEstimatedFT_1 As Double
    Private m_Pilot_2 As String
    Private m_IDX_Pilot_2 As Integer
    Private m_PilotType_2 As String
    Private m_IDX_PilotType_2 As Integer
    Private m_PilotAP_2 As String
    Private m_IDX_PilotAP_2 As Integer
    Private m_PilotEstimatedFT_2 As Double
    Private m_Aircraft As String
    Private m_IDX_Aircraft As Integer
    Private m_AircraftAP As String
    Private m_IDX_AircraftAP As Integer
    Private m_ACType As String
    Private m_IDX_ACType As Integer
    Private m_ACService As Double
    Private m_ACWeight As Integer
    Private m_ACBuyRate As Double
    Private m_ACSellRate As Double
    Private m_ACRangeKM As Integer
    Private m_ACRangeHours As Double
    Private m_ACSpeed As Integer
    Private m_ACMaxPax As Integer
    Private m_ACTurnaroundTime As Integer
    Private m_Revision As Integer
    Private m_EmptyMass As Double
    Private m_FuelFlow As Double
    Private m_ACReserveFuel As Integer
    Private m_ACFuelType As String
    Private m_IDX_ACFuelType As Integer
    Private m_OwnAircraft As Boolean
    Private m_TechlogNumber As Integer
    Private m_IsSignedOff As Boolean
    Private m_HasAircraftAPChanged As Boolean
    Private m_HasPilotAP_1Changed As Boolean
    Private m_HasPilotAP_2Changed As Boolean
    Private m_Notes As String


    'Other
    Private m_GLO_GroupForScheduleList As List(Of GroupForSchedule)
    Private m_IsActiveLeg As Boolean
    Private m_HasChanged As Boolean
    Private m_Edit As Boolean
    Private m_ServerName As String
    Private m_DatabaseName As String
    Private m_Username As String
    Private m_Password As String
    'Collection
    Dim m_Legs As New Collection

#End Region

#Region " System Defined Procedures "

    Public Sub New(serverName As String, databaseName As String, userName As String, passWord As String)

        'SQL
        m_IDX_ACPilot = 0
        m_Pilot_1 = ""
        m_IDX_Pilot_1 = 0
        m_PilotType_1 = ""
        m_IDX_PilotType_1 = 0
        m_PilotAP_1 = ""
        m_IDX_PilotAP_1 = 0
        m_PilotWeight_1 = 0
        m_PilotEstimatedFT_1 = 0.0
        m_Pilot_2 = ""
        m_IDX_Pilot_2 = 0
        m_PilotType_2 = ""
        m_IDX_PilotType_2 = 0
        m_PilotAP_2 = ""
        m_IDX_PilotAP_2 = 0
        m_PilotEstimatedFT_2 = 0.0
        m_Aircraft = ""
        m_IDX_Aircraft = 0
        m_AircraftAP = ""
        m_IDX_AircraftAP = 0
        m_ACType = ""
        m_IDX_ACType = 0
        m_ACService = 0
        m_ACWeight = 0
        m_ACBuyRate = 0
        m_ACSellRate = 0
        m_ACRangeKM = 0
        m_ACRangeHours = 0
        m_ACSpeed = 0
        m_ACMaxPax = 0
        m_ACTurnaroundTime = 0
        m_Revision = 0
        m_EmptyMass = 0
        m_FuelFlow = 0
        m_ACReserveFuel = 0
        m_ACFuelType = ""
        m_IDX_ACFuelType = 0
        m_OwnAircraft = False
        m_TechlogNumber = 0
        m_IsSignedOff = False
        m_HasAircraftAPChanged = False
        m_HasPilotAP_1Changed = False
        m_HasPilotAP_2Changed = False
        m_Notes = ""
        m_ServerName = serverName
        m_DatabaseName = databaseName
        m_Username = userName
        m_Password = passWord

        'Other
        m_IsActiveLeg = False
        m_HasChanged = False
        m_Edit = False

        'Collection
        m_Legs = New Collection

    End Sub

    Public Sub New(ByVal _IDXACPilot As Integer,
                   ByVal _Pilot1 As String,
                   ByVal _IDXPilot1 As Integer,
                   ByVal _Pilot1Type As String,
                   ByVal _IDXPilot1Type As Integer,
                   ByVal _APPilot1 As String,
                   ByVal _IDXAPPilot1 As Integer,
                   ByVal _Pilot1Weight As Integer,
                   ByVal _PilotEstimatedFT_1 As Double,
                   ByVal _Pilot2 As String,
                   ByVal _IDXPilot2 As Integer,
                   ByVal _Pilot2Type As String,
                   ByVal _IDXPilot2Type As Integer,
                   ByVal _APPilot2 As String,
                   ByVal _IDXAPPilot2 As Integer,
                   ByVal _PilotEstimatedFT_2 As Double,
                   ByVal _Aircraft As String,
                   ByVal _IDXAircraft As Integer,
                   ByVal _APAircraft As String,
                   ByVal _IDXAPAircraft As Integer,
                   ByVal _ACType As String,
                   ByVal _IDXACType As Integer,
                   ByVal _ACService As Double,
                   ByVal _ACTypeWeight As Integer,
                   ByVal _ACBuyRate As Double,
                   ByVal _ACSellRate As Double,
                   ByVal _ACTypeRangeKM As Integer,
                   ByVal _ACTypeRangeHours As Double,
                   ByVal _ACTypeSpeed As Integer,
                   ByVal _ACTypeMaxPax As Integer,
                   ByVal _ACTypeTurnTime As Integer,
                   ByVal _Revision As Integer,
                   ByVal _ACEmptyMass As Double,
                   ByVal _ACTypeFuelFlow As Double,
                   ByVal _ACReserveFuel As Integer,
                   ByVal _ACTypeFuelType As String,
                   ByVal _IDXACTypeFuelType As Integer,
                   ByVal _OwnAircraft As Boolean,
                   ByVal _TechlogNumber As Integer,
                   ByVal _SignedOff As Boolean,
                   ByVal _HasACAPChanged As Boolean,
                   ByVal _HasPilotAP_1Changed As Boolean,
                   ByVal _HasPilotAP_2Changed As Boolean,
                   ByVal _Notes As String,
                   Schedule As clsScheduleVariables,
                   ByVal _ServerName As String,
                   ByVal _DatabaseName As String,
                   ByVal _Username As String,
                   ByVal _Password As String,
                   _GLO_GroupForScheduleList As List(Of GroupForSchedule))

        'SQL
        m_IDX_ACPilot = _IDXACPilot
        m_Pilot_1 = _Pilot1
        m_IDX_Pilot_1 = _IDXPilot1
        m_PilotType_1 = _Pilot1Type
        m_IDX_PilotType_1 = _IDXPilot1Type
        m_PilotAP_1 = _APPilot1
        m_IDX_PilotAP_1 = _IDXAPPilot1
        m_PilotWeight_1 = _Pilot1Weight
        m_PilotEstimatedFT_1 = _PilotEstimatedFT_1
        m_PilotEstimatedFT_2 = _PilotEstimatedFT_2
        m_Pilot_2 = _Pilot2
        m_IDX_Pilot_2 = _IDXPilot2
        m_PilotType_2 = _Pilot2Type
        m_IDX_PilotType_2 = _IDXPilot2Type
        m_PilotAP_2 = _APPilot2
        m_IDX_PilotAP_2 = _IDXAPPilot2
        m_Aircraft = _Aircraft
        m_IDX_Aircraft = _IDXAircraft
        m_AircraftAP = _APAircraft
        m_IDX_AircraftAP = _IDXAPAircraft
        m_ACType = _ACType
        m_IDX_ACType = _IDXACType
        m_ACService = _ACService
        m_ACWeight = _ACTypeWeight
        m_ACBuyRate = _ACBuyRate
        m_ACSellRate = _ACSellRate
        m_ACRangeKM = _ACTypeRangeKM
        m_ACRangeHours = _ACTypeRangeHours
        m_ACSpeed = _ACTypeSpeed
        m_ACMaxPax = _ACTypeMaxPax
        m_ACTurnaroundTime = _ACTypeTurnTime
        m_Revision = _Revision
        m_EmptyMass = _ACEmptyMass
        m_FuelFlow = _ACTypeFuelFlow
        m_ACReserveFuel = _ACReserveFuel
        m_ACFuelType = _ACTypeFuelType
        m_IDX_ACFuelType = _IDXACTypeFuelType
        m_OwnAircraft = _OwnAircraft
        m_TechlogNumber = _TechlogNumber
        m_IsSignedOff = _SignedOff
        m_HasAircraftAPChanged = _HasACAPChanged
        m_HasPilotAP_1Changed = _HasPilotAP_1Changed
        m_HasPilotAP_2Changed = _HasPilotAP_2Changed
        m_Notes = _Notes
        m_ServerName = _ServerName
        m_DatabaseName = _DatabaseName
        m_Username = _Username
        m_Password = _Password
        m_GLO_GroupForScheduleList = _GLO_GroupForScheduleList
        'Other
        m_IsActiveLeg = True
        m_HasChanged = False
        m_Edit = True

        'Collection
        m_Legs = New Collection

        '--Retrieve Scheduled legs.--

        'Dim SQLCmd As String = ""
        'Dim SQLParam(0) As SqlParameter
        'Dim SQLds As DataSet

        'SQLCmd = "[VIEW].[sl_ScheduleLegs]"
        'SQLParam(0) = New SqlParameter("@IDXACPilot", IDX_ACPilot)
        'SQLds = New DataSet

        'ExecuteStoredProc(SQLCmd, 20, SQLParam, SQLds, _ServerName, _DatabaseName, _Username, _Password)

        'If CheckDataset(SQLds) Then
        '    Legs = GetListFromTable(SQLds.Tables(0), New clsScheduleLegs, Legs, _ServerName, _DatabaseName, _Username, _Password, m_GLO_GroupForScheduleList)
        'End If

        'CalculateFuelWT_RoughEstimate(Schedule)
        'CalculateTotalWT()
        'AddNextRoute(Schedule)
        'SetHasEtdChanged()

    End Sub

#End Region

#Region " Property "

#Region " Property - SQL Columns "


    Public Property IDX_ACPilot() As Integer
        Get
            IDX_ACPilot = m_IDX_ACPilot
        End Get
        Set(ByVal value As Integer)
            m_IDX_ACPilot = value
        End Set
    End Property

    Public Property Pilot_1() As String
        Get
            Pilot_1 = m_Pilot_1
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                    (Not value = m_Pilot_1) Then
                m_HasChanged = True
            End If

            m_Pilot_1 = value
        End Set
    End Property

    Public Property IDX_Pilot_1() As Integer
        Get
            IDX_Pilot_1 = m_IDX_Pilot_1
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                    (Not value = m_IDX_Pilot_1) Then
                m_HasChanged = True
            End If

            m_IDX_Pilot_1 = value
        End Set
    End Property

    Public Property PilotType_1() As String
        Get
            PilotType_1 = m_PilotType_1
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                    (Not value = m_PilotType_1) Then
                m_HasChanged = True
            End If

            m_PilotType_1 = value
        End Set
    End Property

    Public Property IDX_PilotType_1() As Integer
        Get
            IDX_PilotType_1 = m_IDX_PilotType_1
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                    (Not value = m_IDX_PilotType_1) Then
                m_HasChanged = True
            End If

            m_IDX_PilotType_1 = value
        End Set
    End Property

    Public Property PilotAP_1() As String
        Get
            PilotAP_1 = m_PilotAP_1
        End Get
        Set(ByVal value As String)
            m_PilotAP_1 = value
        End Set
    End Property

    Public Property IDX_PilotAP_1() As Integer
        Get
            IDX_PilotAP_1 = m_IDX_PilotAP_1
        End Get
        Set(ByVal value As Integer)
            m_IDX_PilotAP_1 = value
        End Set
    End Property

    Public Property PilotWeight_1() As Integer
        Get
            PilotWeight_1 = m_PilotWeight_1
        End Get
        Set(ByVal value As Integer)
            m_PilotWeight_1 = value
        End Set
    End Property

    Public Property PilotEstimatedFT_1() As Double
        Get
            PilotEstimatedFT_1 = Math.Round(m_PilotEstimatedFT_1, 2)
        End Get
        Set(ByVal value As Double)
            m_PilotEstimatedFT_1 = value
        End Set
    End Property

    Public Property Pilot_2() As String
        Get
            Pilot_2 = m_Pilot_2
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                    (Not value = m_Pilot_2) Then
                m_HasChanged = True
            End If

            m_Pilot_2 = value
        End Set
    End Property

    Public Property IDX_Pilot_2() As Integer
        Get
            IDX_Pilot_2 = m_IDX_Pilot_2
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                    (Not value = m_IDX_Pilot_2) Then
                m_HasChanged = True
            End If

            m_IDX_Pilot_2 = value
        End Set
    End Property

    Public Property PilotType_2() As String
        Get
            PilotType_2 = m_PilotType_2
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                    (Not value = m_PilotType_2) Then
                m_HasChanged = True
            End If

            m_PilotType_2 = value
        End Set
    End Property

    Public Property IDX_PilotType_2() As Integer
        Get
            IDX_PilotType_2 = m_IDX_PilotType_2
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                    (Not value = m_IDX_PilotType_2) Then
                m_HasChanged = True
            End If

            m_IDX_PilotType_2 = value
        End Set
    End Property

    Public Property PilotAP_2() As String
        Get
            PilotAP_2 = m_PilotAP_2
        End Get
        Set(ByVal value As String)
            m_PilotAP_2 = value
        End Set
    End Property

    Public Property IDX_PilotAP_2() As Integer
        Get
            IDX_PilotAP_2 = m_IDX_PilotAP_2
        End Get
        Set(ByVal value As Integer)
            m_IDX_PilotAP_2 = value
        End Set
    End Property

    Public Property PilotEstimatedFT_2() As Double
        Get
            PilotEstimatedFT_2 = Math.Round(m_PilotEstimatedFT_2, 2)
        End Get
        Set(ByVal value As Double)
            m_PilotEstimatedFT_2 = value
        End Set
    End Property

    Public Property Aircraft() As String
        Get
            Aircraft = m_Aircraft
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                (Not value = m_Aircraft) Then
                m_HasChanged = True
            End If

            m_Aircraft = value
        End Set
    End Property

    Public Property IDX_Aircraft() As Integer
        Get
            IDX_Aircraft = m_IDX_Aircraft
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                (Not value = m_IDX_Aircraft) Then
                m_HasChanged = True
            End If

            m_IDX_Aircraft = value
        End Set
    End Property

    Public Property AircraftAP() As String
        Get
            AircraftAP = m_AircraftAP
        End Get
        Set(ByVal value As String)
            m_AircraftAP = value
        End Set
    End Property

    Public Property IDX_AircraftAP() As Integer
        Get
            IDX_AircraftAP = m_IDX_AircraftAP
        End Get
        Set(ByVal value As Integer)
            m_IDX_AircraftAP = value
        End Set
    End Property

    Public Property ACType() As String
        Get
            ACType = m_ACType
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                (Not value = m_ACType) Then
                m_HasChanged = True
            End If

            m_ACType = value
        End Set
    End Property

    Public Property IDX_ACType() As Integer
        Get
            IDX_ACType = m_IDX_ACType
        End Get
        Set(ByVal value As Integer)

            If (m_Edit = True) And _
                (Not value = m_IDX_ACType) Then
                m_HasChanged = True
            End If

            m_IDX_ACType = value
        End Set
    End Property

    Public Property ACService() As Double
        Get
            ACService = Math.Round(m_ACService, 2)
        End Get
        Set(ByVal value As Double)
            m_ACService = value
        End Set
    End Property

    Public Property ACWeight() As Integer
        Get
            ACWeight = m_ACWeight
        End Get
        Set(ByVal value As Integer)
            m_ACWeight = value
        End Set
    End Property

    Public Property ACBuyRate() As Double
        Get
            ACBuyRate = Math.Round(m_ACBuyRate, 2)
        End Get
        Set(ByVal value As Double)
            m_ACBuyRate = value
        End Set
    End Property

    Public Property ACSellRate() As Double
        Get
            ACSellRate = Math.Round(m_ACSellRate, 2)
        End Get
        Set(ByVal value As Double)
            m_ACSellRate = value
        End Set
    End Property

    Public Property ACRangeKM() As Integer
        Get
            ACRangeKM = m_ACRangeKM
        End Get
        Set(ByVal value As Integer)
            m_ACRangeKM = value
        End Set
    End Property

    Public Property ACRangeHours() As Double
        Get
            ACRangeHours = Math.Round(m_ACRangeHours, 2)
        End Get
        Set(ByVal value As Double)
            m_ACRangeHours = value
        End Set
    End Property

    Public Property ACSpeed() As Integer
        Get
            ACSpeed = m_ACSpeed
        End Get
        Set(ByVal value As Integer)
            m_ACSpeed = value
        End Set
    End Property

    Public Property ACMaxPax() As Integer
        Get
            ACMaxPax = m_ACMaxPax
        End Get
        Set(ByVal value As Integer)
            m_ACMaxPax = value
        End Set
    End Property

    Public Property ACTurnaroundTime() As Integer
        Get
            ACTurnaroundTime = m_ACTurnaroundTime
        End Get
        Set(ByVal value As Integer)
            m_ACTurnaroundTime = value
        End Set
    End Property

    Public Property Revision() As Integer
        Get
            Revision = m_Revision
        End Get
        Set(ByVal value As Integer)
            m_Revision = value
        End Set
    End Property

    Public Property EmptyMass() As Double
        Get
            EmptyMass = Math.Round(m_EmptyMass, 2)
        End Get
        Set(ByVal value As Double)
            m_EmptyMass = value
        End Set
    End Property

    Public Property FuelFlow() As Double
        Get
            FuelFlow = Math.Round(m_FuelFlow, 2)
        End Get
        Set(ByVal value As Double)
            m_FuelFlow = value
        End Set
    End Property

    Public Property ACReserveFuel() As Integer
        Get
            Return m_ACReserveFuel
        End Get
        Set(ByVal value As Integer)
            m_ACReserveFuel = value
        End Set
    End Property

    Public Property ACFuelType() As String
        Get
            Return m_ACFuelType
        End Get
        Set(ByVal value As String)
            m_ACFuelType = value
        End Set
    End Property

    Public Property IDX_ACFuelType() As Integer
        Get
            Return m_IDX_ACFuelType
        End Get
        Set(ByVal value As Integer)
            m_IDX_ACFuelType = value
        End Set
    End Property

    Public Property OwnAircraft() As Boolean
        Get
            Return m_OwnAircraft
        End Get
        Set(ByVal value As Boolean)
            m_OwnAircraft = value
        End Set
    End Property

    Public Property TechlogNumber() As Integer
        Get
            TechlogNumber = m_TechlogNumber
        End Get
        Set(ByVal value As Integer)
            m_TechlogNumber = value
        End Set
    End Property

    Public Property IsSignedOff() As Boolean
        Get
            IsSignedOff = m_IsSignedOff
        End Get
        Set(ByVal value As Boolean)
            m_IsSignedOff = value
        End Set
    End Property

    Public Property HasAircraftAPChanged() As Boolean
        Get
            HasAircraftAPChanged = m_HasAircraftAPChanged
        End Get
        Set(ByVal value As Boolean)
            m_HasAircraftAPChanged = value
        End Set
    End Property

    Public Property HasPilotAP_1Changed() As Boolean
        Get
            HasPilotAP_1Changed = m_HasPilotAP_1Changed
        End Get
        Set(ByVal value As Boolean)
            m_HasPilotAP_1Changed = value
        End Set
    End Property

    Public Property HasPilotAP_2Changed() As Boolean
        Get
            HasPilotAP_2Changed = m_HasPilotAP_2Changed
        End Get
        Set(ByVal value As Boolean)
            m_HasPilotAP_2Changed = value
        End Set
    End Property

    Public Property Notes() As String
        Get
            Notes = m_Notes
        End Get
        Set(ByVal value As String)

            If (m_Edit = True) And _
                    (Not value = m_Notes) Then
                m_HasChanged = True
            End If

            m_Notes = value
        End Set
    End Property

#End Region

#Region " Property - Other "

    Public Property IsActiveLeg() As Boolean
        Get
            IsActiveLeg = m_IsActiveLeg
        End Get
        Set(ByVal value As Boolean)

            If value And Legs.Count = 0 Then
                Dim tmp_cls As New clsScheduleLegs

                tmp_cls.Etd = "07:00"
                Legs.Add(tmp_cls)
            End If

            m_IsActiveLeg = value
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

#End Region

#Region " Property - Collection "

    Public ReadOnly Property Legs(ByVal i As Integer) As clsScheduleLegs
        Get
            If i > 0 Then
                Legs = m_Legs(i)
            Else
                Legs = m_Legs(1)
            End If
        End Get
    End Property

    Public Property Legs() As Collection
        Get
            Legs = m_Legs
        End Get
        Set(ByVal Value As Collection)
            m_Legs = Value
        End Set
    End Property

#End Region

#Region " Property - Calculated "

    Friend ReadOnly Property ACTotalFlightTime() As Long
        Get
            If Legs.Count > 0 Then
                For i As Integer = 1 To Legs.Count
                    ACTotalFlightTime += Legs(i).LegFlightTime
                Next i
            Else
                ACTotalFlightTime = 0
            End If

            Return ACTotalFlightTime
        End Get
    End Property

    Friend ReadOnly Property ACTotalDistance() As Integer
        Get
            If Legs.Count > 0 Then
                For i As Integer = 1 To Legs.Count
                    ACTotalDistance += Legs(i).Distance
                Next i
            Else
                ACTotalDistance = 0
            End If
            Return ACTotalDistance
        End Get
    End Property

    Friend ReadOnly Property ACTotalSellRate() As Double
        Get
            If Legs.Count > 0 Then
                For i As Integer = 1 To Legs.Count
                    ACTotalSellRate += Legs(i).LegSellRate(ACSellRate)
                Next i
            Else
                ACTotalSellRate = 0
            End If

            Return ACTotalSellRate
        End Get
    End Property

    Friend ReadOnly Property ACTotalPaxKM() As Double
        Get
            If Legs.Count > 0 Then
                For i As Integer = 1 To Legs.Count
                    ACTotalPaxKM += Legs(i).LegPaxKM
                Next i
            Else
                ACTotalPaxKM = 0
            End If
            Return ACTotalPaxKM
        End Get
    End Property

#End Region

#End Region

#Region " User Defined Procedures "


    Public Sub Recalculate(Schedule As clsScheduleVariables)
        CalculateFuelWT_RoughEstimate(Schedule)
        CalculateTotalWT()
        AddNextRoute(Schedule)
        SetHasEtdChanged()
    End Sub
    Public Function CopyFromList(ByVal params() As Object, Schedule As clsScheduleVariables,
                                 ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As clsScheduleACPilot
        Return New clsScheduleACPilot(params(0), params(1), params(2), params(3), params(4), params(5) _
                                      , params(6), params(7), params(8), params(9), params(10) _
                                      , params(11), params(12), params(13), params(14), params(15) _
                                      , params(16), params(17), params(18), params(19), params(20) _
                                      , params(21), params(22), params(23), params(24), params(25) _
                                      , params(26), params(27), params(28), params(29), params(30) _
                                      , params(31), params(32), params(33), params(34), params(35) _
                                      , params(36), params(37), params(38), params(39), params(40) _
                                      , params(41), params(42), params(43), Schedule, ServerName, DatabaseName, Username, Password, GLO_GroupForScheduleList
                                      )
    End Function
    Public Function NewCopy(ByVal params() As Object, ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy Implements ICopy.NewCopy

        Return New clsScheduleACPilot(params(0), params(1), params(2), params(3), params(4), params(5) _
                                      , params(6), params(7), params(8), params(9), params(10) _
                                      , params(11), params(12), params(13), params(14), params(15) _
                                      , params(16), params(17), params(18), params(19), params(20) _
                                      , params(21), params(22), params(23), params(24), params(25) _
                                      , params(26), params(27), params(28), params(29), params(30) _
                                      , params(31), params(32), params(33), params(34), params(35) _
                                      , params(36), params(37), params(38), params(39), params(40) _
                                      , params(41), params(42), params(43), params(44), ServerName, DatabaseName, Username, Password, GLO_GroupForScheduleList
                                      )

    End Function

    Public Function RowObjects() As Object()

        Return New Object() {IDX_ACPilot, _
                             " ", _
                             Pilot_1, _
                             IDX_Pilot_1, _
                             PilotType_1, _
                             IDX_PilotType_1, _
                             PilotAP_1, _
                             PilotEstimatedFT_1, _
                             PilotWeight_1, _
                             Pilot_2, _
                             IDX_Pilot_2, _
                             PilotType_2, _
                             IDX_PilotType_2, _
                             PilotAP_2, _
                             PilotEstimatedFT_2, _
                             Aircraft, _
                             IDX_Aircraft, _
                             AircraftAP, _
                             ACType, _
                             ACService, _
                             ACWeight, _
                             ACBuyRate, _
                             ACSellRate, _
                             ACRangeKM, _
                             ACRangeHours, _
                             ACSpeed, _
                             ACMaxPax, _
                             ACTurnaroundTime, _
                             Revision, _
                             EmptyMass, _
                             FuelFlow, _
                             ACReserveFuel, _
                             ACFuelType, _
                             TechlogNumber, _
                             IsSignedOff, _
                             IsActiveLeg}

    End Function

    Public Sub CalculateFuelWT_RoughEstimate(Schedule As clsScheduleVariables)

        '--Variables.--

        Dim tmp_LastLeg As Integer = 0

        '--Find the last row of schedule.--

        If Legs.Count > 1 Then
            If Legs(Legs.Count).IDX_FromAP > 0 And Legs(Legs.Count).IDX_ToAP > 0 Then
                tmp_LastLeg = Legs.Count
            Else
                tmp_LastLeg = Legs.Count - 1
            End If
        Else
            tmp_LastLeg = Legs.Count
        End If

        'Calculate fuel needed per leg.
        For i As Integer = 1 To tmp_LastLeg
            Legs(i).CalculateLegFuelWT(m_FuelFlow, m_ACSpeed)
        Next i

        'Calculate the fuel needed between refuel stops.
        For ii As Integer = tmp_LastLeg To 1 Step -1

            Legs(ii).TotalFuelWT = 0
            Legs(ii).LegFuelWTOther = 0

            If Legs(ii).ToRefuel = True Then

                Legs(ii).TotalFuelWT = Legs(ii).ConFuelWT + Legs(ii).AltAPFuelWT + Legs(ii).LegFuelWT + ACReserveFuel

            ElseIf Legs(ii).ToRefuel = False Then

                If tmp_LastLeg = ii Then
                    Legs(ii).LegFuelWTOther = GetAlternateReFuelWT(Schedule, Legs(ii).IDX_ToAP)
                Else
                    Legs(ii).LegFuelWTOther = Legs(ii + 1).TotalFuelWT - ACReserveFuel - Legs(ii).AltAPFuelWT
                End If

                Legs(ii).TotalFuelWT = Legs(ii).LegFuelWTOther + Legs(ii).ConFuelWT + Legs(ii).AltAPFuelWT + Legs(ii).LegFuelWT + ACReserveFuel
            End If

        Next ii

    End Sub

    Public Sub CalculateFuelWT_ExactEstimate(Schedule As clsScheduleVariables, Optional ByVal _IDX_AP As Integer = -1, Optional ByVal _OtherFuelWT As Double = 0.0)

        '--Variables.--

        Dim tmp_LastLeg As Integer = 0

        '--Find the last row of schedule.--

        If Legs.Count > 1 Then
            If Legs(Legs.Count).IDX_FromAP > 0 And Legs(Legs.Count).IDX_ToAP > 0 Then
                tmp_LastLeg = Legs.Count
            Else
                tmp_LastLeg = Legs.Count - 1
            End If
        Else
            tmp_LastLeg = Legs.Count
        End If

        'Calculate fuel needed per leg between two airstrips.
        For i As Integer = 1 To tmp_LastLeg
            Legs(i).CalculateLegFuelWT(m_FuelFlow, m_ACSpeed)
        Next i

        'Calculate fuel weight between refuel stops.

        For ii As Integer = tmp_LastLeg To 1 Step -1

            'Reset values.
            Legs(ii).TotalFuelWT = 0
            Legs(ii).LegFuelWTOther = 0

            'Check Fuel point.
            If Legs(ii).ToRefuel Then
                Legs(ii).TotalFuelWT = Legs(ii).ConFuelWT + Legs(ii).AltAPFuelWT + Legs(ii).LegFuelWT + ACReserveFuel
            ElseIf Not Legs(ii).ToRefuel Then

                If tmp_LastLeg = ii Then
                    If _IDX_AP < 0 Then
                        Legs(ii).LegFuelWTOther = 0

                        Dim SQLCmd As String
                        Dim SQLParam(1) As SqlParameter
                        Dim SQLds As DataSet

                        SQLCmd = "[VIEW].[sl_ScheduleNextDayStart]"
                        SQLParam(0) = New SqlParameter("@FlightDate", Schedule.VAR_OpenDate.ToString("dd/MMM/yyyy"))
                        SQLParam(1) = New SqlParameter("@IDX_AC_Pilot", IDX_Aircraft)
                        SQLds = New DataSet

                        ExecuteStoredProc(SQLCmd, 0, SQLParam, SQLds, m_ServerName, m_DatabaseName, m_Username, m_Password)

                        If CheckDataset(SQLds) Then
                            Legs(ii).LegFuelWTOther = SQLds.Tables(0).Rows(0).Item("FuelWT")
                        Else
                            Legs(ii).LegFuelWTOther = GetAlternateReFuelWT(Schedule, Legs(ii).IDX_ToAP)
                        End If
                    Else
                        Legs(ii).LegFuelWTOther = _OtherFuelWT
                    End If

                Else
                    Legs(ii).LegFuelWTOther = Legs(ii + 1).TotalFuelWT - ACReserveFuel - Legs(ii).AltAPFuelWT
                End If

                Legs(ii).TotalFuelWT = Legs(ii).LegFuelWTOther + Legs(ii).ConFuelWT + Legs(ii).AltAPFuelWT + Legs(ii).LegFuelWT + ACReserveFuel
            End If

        Next ii

    End Sub

    ''' <summary>
    ''' Calculate the closest refuel point.
    ''' </summary>
    ''' <param name="_IDX_AP">Integer: Airport ID.</param>
    ''' <returns>Double: The fuel load in lb.</returns>
    ''' <remarks></remarks>
    Public Function GetAlternateReFuelWT(Schedule As clsScheduleVariables, ByVal _IDX_AP As Integer) As Double

        '--Variables.--

        Dim tmp_Distance As Double
        Dim tmp_FT As Double

        '--Get closest refuel point.--
        tmp_Distance = AirportClosestFuel.GetDistance(IDX_ACFuelType, _IDX_AP, _IDX_AP)

        '--Calculate the fuel needed to get too closest refuel point.--
        tmp_FT = GetFlightTime(tmp_Distance, ACSpeed)

        If tmp_FT > 0 Then
            Return (FuelFlow * (tmp_FT / 60))
        Else
            Return 0
        End If


    End Function

    Public Sub CalculateTotalWT()

        For i As Integer = 1 To Legs.Count

            Legs(i).CalculatePassengerWT(PilotWeight_1)
            Legs(i).MTOW = Legs(i).TotalFuelWT + Legs(i).TotalPaxWT + EmptyMass
            Legs(i).MLW = (Legs(i).TotalFuelWT - Legs(i).LegFuelWT - Legs(i).ConFuelWT) + Legs(i).TotalPaxWT + EmptyMass

            Dim tmp_TOWDiff As Double = 0.0
            Dim tmp_LWDiff As Double = 0.0
            Dim tmp_ACWTDiff As Double = 0.0

            tmp_TOWDiff = Legs(i).FromMTOW - Legs(i).MTOW
            tmp_LWDiff = Legs(i).ToMLW - Legs(i).MLW
            tmp_ACWTDiff = ACWeight - Legs(i).MTOW

            If tmp_LWDiff <= tmp_TOWDiff Then
                If tmp_ACWTDiff <= tmp_LWDiff Then
                    Legs(i).TotalAvailWT = tmp_ACWTDiff
                ElseIf tmp_ACWTDiff > tmp_LWDiff Then
                    Legs(i).TotalAvailWT = tmp_LWDiff
                End If
            ElseIf tmp_LWDiff > tmp_TOWDiff Then
                If tmp_ACWTDiff <= tmp_TOWDiff Then
                    Legs(i).TotalAvailWT = tmp_ACWTDiff
                ElseIf tmp_ACWTDiff > tmp_TOWDiff Then
                    Legs(i).TotalAvailWT = tmp_TOWDiff
                End If
            End If

        Next i

    End Sub

    Public Sub SetHasEtdChanged()

        For i As Integer = 1 To Legs.Count

            Legs(i).HasEtdChanged = False

            If i > 1 Then

                Dim tmp_prevEta As DateTime = CDate(Legs(i - 1).Eta)
                Dim tmp_etd As DateTime = DateAdd(DateInterval.Minute, ACTurnaroundTime + Legs(i).FromTurnTime, tmp_prevEta)
                tmp_etd = RoundETA_ETD(True, False, tmp_etd)

                If tmp_etd <> Legs(i).Etd Then Legs(i).HasEtdChanged = True

            End If

        Next i

    End Sub

    Public Sub AddNextRoute(Schedule As clsScheduleVariables)

        If TechlogNumber > 0 Or IsSignedOff Then Exit Sub

        If Legs.Count = 0 Then Exit Sub

        If Legs(Legs.Count).IDX_ToAP <= 0 Then Exit Sub

        Dim tmp_prevAP As String = ""
        Dim tmp_prevIDX_AP As Integer = 0
        Dim tmp_prevTurnTime As Integer = 0.0
        Dim tmp_TOWT As Double = 0.0
        Dim tmp_prevFuelPoint As Boolean = False
        Dim tmp_prevETA As String
        Dim tmp_etd As DateTime

        tmp_prevAP = Legs(Legs.Count).ToAP
        tmp_prevIDX_AP = Legs(Legs.Count).IDX_ToAP
        tmp_prevFuelPoint = Legs(Legs.Count).ToRefuel
        tmp_prevTurnTime = Legs(Legs.Count).ToTurnTime
        tmp_prevETA = Legs(Legs.Count).Eta
        tmp_etd = DateAdd(DateInterval.Minute, tmp_prevTurnTime + ACTurnaroundTime, CDate(tmp_prevETA))


        'To Airport.
        Dim tmpAPLimits As ACAirportLimits = ACAirportLimits.GetAPLimits(tmp_prevIDX_AP, IDX_ACType, Schedule.VAR_OpenDate, Schedule.VAR_OpenDate)

        If (tmpAPLimits IsNot Nothing) Then
            tmp_TOWT = tmpAPLimits.MaxTakeOffWeight
        End If

        Dim tmp_cls As New clsScheduleLegs(0,
                                           IDX_ACPilot,
                                           tmp_etd.ToString("HH:mm"),
                                           tmp_prevAP,
                                           tmp_prevIDX_AP,
                                           tmp_prevFuelPoint,
                                           tmp_prevTurnTime,
                                           "",
                                           "",
                                           0,
                                           False,
                                           0,
                                           0,
                                           0,
                                           0,
                                           tmp_TOWT,
                                           0,
                                           "",
                                           0,
                                           0,
                                           Aircraft,
                                           m_ServerName,
                                           m_DatabaseName,
                                           m_Username,
                                           m_Password,
                                           m_GLO_GroupForScheduleList)

        Legs.Add(tmp_cls)

    End Sub

#End Region

End Class
