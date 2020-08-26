Imports Schedwin.Common.OldVB

Public Class clsScheduleLegRes
    Implements ICopy

#Region " Variables "

    'SQL Columns
    Dim m_IDX_LegRes As Integer
    Dim m_IDX_Leg As Integer
    Dim m_IDX_ACPilot As Integer
    Dim m_IDX_RL As Integer
    Dim m_IDX_RH As Integer
    Dim m_Reservation As String
    Dim m_Pax As Integer
    Dim m_PaxWT As Double
    Dim m_LugWT As Double

#End Region

#Region " System Defined Procedures "

    Public Sub New()

        m_IDX_LegRes = 0
        m_IDX_Leg = 0
        m_IDX_ACPilot = 0
        m_IDX_RL = 0
        m_IDX_RH = 0
        m_Reservation = ""
        m_Pax = 0
        m_PaxWT = 0
        m_LugWT = 0

    End Sub

    Public Sub New(ByVal _IDX_LegRes As Integer,
                   ByVal _IDX_Leg As Integer,
                   ByVal _IDXACPilot As Integer,
                   ByVal _IDX_RL As Integer,
                   ByVal _IDX_RH As Integer,
                   ByVal _ResName As String,
                   ByVal _Numpax As Integer,
                   ByVal _ResWeight As Double,
                   ByVal _LuggageWeight As Double)

        m_IDX_LegRes = _IDX_LegRes
        m_IDX_Leg = _IDX_Leg
        m_IDX_ACPilot = _IDXACPilot
        m_IDX_RL = _IDX_RL
        m_IDX_RH = _IDX_RH
        m_Reservation = _ResName
        m_Pax = _Numpax
        m_PaxWT = _ResWeight
        m_LugWT = _LuggageWeight

    End Sub

#End Region

#Region " Property "

#Region " Property - SQL Columns "

    Public Property IDX_LegRes() As Integer
        Get
            IDX_LegRes = m_IDX_LegRes
        End Get
        Set(ByVal value As Integer)
            m_IDX_LegRes = value
        End Set
    End Property

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

    Public Property IDX_RL() As Integer
        Get
            IDX_RL = m_IDX_RL
        End Get
        Set(ByVal value As Integer)
            m_IDX_RL = value
        End Set
    End Property

    Public Property IDX_RH() As Integer
        Get
            IDX_RH = m_IDX_RH
        End Get
        Set(ByVal value As Integer)
            m_IDX_RH = value
        End Set
    End Property

    Public Property Reservation() As String
        Get
            Reservation = m_Reservation
        End Get
        Set(ByVal value As String)
            m_Reservation = value
        End Set
    End Property

    Public Property Pax() As Integer
        Get
            Pax = m_Pax
        End Get
        Set(ByVal value As Integer)
            m_Pax = value
        End Set
    End Property

    Public Property PaxWeight() As Double
        Get
            PaxWeight = Math.Round(m_PaxWT, 2)
        End Get
        Set(ByVal value As Double)
            m_PaxWT = value
        End Set
    End Property

    Public Property LugWeight() As Double
        Get
            LugWeight = Math.Round(m_LugWT, 2)
        End Get
        Set(ByVal value As Double)
            m_LugWT = value
        End Set
    End Property

#End Region

#End Region

#Region " User Defined Propcedures "

    Public Function NewCopy(ByVal params() As Object, ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy Implements ICopy.NewCopy

        Return New clsScheduleLegRes(params(0), params(1), params(2), params(3), params(4),
                                     params(5), params(6), params(7), params(8))

    End Function

    Public Function FindIndexGFSByIDX_RL(ByVal _Group As GroupForSchedule) As Boolean
        Return _Group.IDX_RL = IDX_RL
    End Function

#End Region

End Class
