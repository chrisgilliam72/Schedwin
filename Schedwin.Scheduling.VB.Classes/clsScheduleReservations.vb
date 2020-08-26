Imports System.Collections.Generic
Imports Schedwin.Common.OldVB

Public Class clsScheduleReservations
    Implements ICopy

#Region " Variables "
    Public m_GLO_GroupForScheduleList As List(Of GroupForSchedule)
    Public GLO_GroupForSchedule As GroupForSchedule
    Private m_ResSelected As Boolean
    Private m_ResFlag As String
    Private m_ResFromAP As String
    Private m_ResIDX_FromAP As Integer
    Private m_ResToAP As String
    Private m_ResIDX_ToAP As Integer
    Private m_ResPax As Integer
    Private m_ResName As String
    Private m_ResEx As String
    Private m_ResFor As String
    Private m_ResOperator As String
    Private m_ResIDX_RH As Integer
    Private m_ResIDX_RL As Integer
    Private m_ResPassengerWeight As Integer
    Private m_ResLuggageWeight As Integer
    Private m_ResLastDest As String
    Private m_ResSoleUse As Boolean
    Private m_ResACType As String
    Private m_ResIDX_ACType As Integer
    Private m_ResEarlyEx As DateTime
    Private m_ResLateEx As DateTime
    Private m_ResEarlyFor As DateTime
    Private m_ResLateFor As DateTime
    Private m_ResGameFlight As Integer
    Private m_ResCancelled As Boolean
    Private m_ResType As String
    Private m_ResNotes As String

#End Region

#Region " Property "

    Public Property ResEarlyEx() As DateTime
        Get
            Return m_ResEarlyEx
        End Get
        Set(ByVal value As DateTime)
            m_ResEarlyEx = value
        End Set
    End Property

    Public ReadOnly Property ResNotes() As String
        Get
            Return m_ResNotes
        End Get
    End Property

    Public ReadOnly Property ResType() As String
        Get
            Return m_ResType
        End Get
    End Property

    Public ReadOnly Property ResCancelled() As Boolean
        Get
            Return m_ResCancelled
        End Get
    End Property

    Public Property ResLateFor() As DateTime
        Get
            Return m_ResLateFor
        End Get
        Set(ByVal value As DateTime)
            m_ResLateFor = value
        End Set
    End Property

    Public Property ResEarlyFor() As DateTime
        Get
            Return m_ResEarlyFor
        End Get
        Set(ByVal value As DateTime)
            m_ResEarlyFor = value
        End Set
    End Property

    Public Property ResLateEx() As DateTime
        Get
            Return m_ResLateEx
        End Get
        Set(ByVal value As DateTime)
            m_ResLateEx = value
        End Set
    End Property

    Public ReadOnly Property ResIDX_ACType() As Integer
        Get
            Return m_ResIDX_ACType
        End Get
    End Property

    Public ReadOnly Property ResACType() As String
        Get
            Return m_ResACType
        End Get
    End Property

    Public ReadOnly Property ResSoleUse() As Boolean
        Get
            Return m_ResSoleUse
        End Get
    End Property

    Public Property ResLastDest() As String
        Get
            Return m_ResLastDest
        End Get
        Set(ByVal value As String)
            m_ResLastDest = value
        End Set
    End Property

    Public ReadOnly Property ResLuggageWeight() As Integer
        Get
            Return m_ResLuggageWeight
        End Get
    End Property

    Public ReadOnly Property ResPassengerWeight() As Integer
        Get
            Return m_ResPassengerWeight
        End Get
    End Property

    Public ReadOnly Property ResIDX_RL() As Integer
        Get
            Return m_ResIDX_RL
        End Get
    End Property

    Public ReadOnly Property ResIDX_RH() As Integer
        Get
            Return m_ResIDX_RH
        End Get
    End Property

    Public ReadOnly Property ResOperator() As String
        Get
            Return m_ResOperator
        End Get
    End Property

    Public ReadOnly Property ResFor() As String
        Get
            Return m_ResFor
        End Get
    End Property

    Public ReadOnly Property ResEx() As String
        Get
            Return m_ResEx
        End Get
    End Property

    Public ReadOnly Property ResName() As String
        Get
            Return m_ResName
        End Get
    End Property

    Public ReadOnly Property ResPax() As Integer
        Get
            Return m_ResPax
        End Get
    End Property

    Public ReadOnly Property ResIDX_ToAP() As Integer
        Get
            Return m_ResIDX_ToAP
        End Get
    End Property

    Public ReadOnly Property ResToAP() As String
        Get
            Return m_ResToAP
        End Get
    End Property

    Public ReadOnly Property ResIDX_FromAP() As Integer
        Get
            Return m_ResIDX_FromAP
        End Get
    End Property

    Public ReadOnly Property ResFromAP() As String
        Get
            Return m_ResFromAP
        End Get
    End Property

    Public Property ResFlag() As String
        Get
            Return m_ResFlag
        End Get
        Set(ByVal value As String)
            m_ResFlag = value
        End Set
    End Property

    Public Property ResSelected() As Boolean
        Get
            Return m_ResSelected
        End Get
        Set(ByVal value As Boolean)
            m_ResSelected = value
        End Set
    End Property

    Public Property ResGameFlight() As Integer
        Get
            Return m_ResGameFlight
        End Get
        Set(ByVal value As Integer)
            m_ResGameFlight = value
        End Set
    End Property

#End Region

#Region " User Defined Procedures "

    Public Function NewCopy(ByVal params() As Object, ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy Implements ICopy.NewCopy

        Return New clsScheduleReservations(params(0), params(1), params(2), params(3), params(4), params(5) _
                                      , params(6), params(7), params(8), params(9), params(10) _
                                      , params(11), params(12), params(13), params(14), params(15) _
                                      , params(16), params(17), params(18), params(19), params(20) _
                                      , params(21), params(22), params(23), params(24), params(25) _
                                      , params(26), params(27))

    End Function

    Public Function RowObjects() As Object()

        Return New Object() {ResSelected, ResFlag, _
                             ResFromAP, ResIDX_FromAP, _
                             ResToAP, ResIDX_ToAP, _
                             ResPax, ResName, _
                             ResEx, ResFor, _
                             ResOperator, ResIDX_RH, _
                             ResIDX_RL, ResPassengerWeight, _
                             ResLuggageWeight, ResLastDest, _
                             ResSoleUse, ResACType, _
                             ResIDX_ACType, ResCancelled, _
                             ResEarlyEx.ToString("HH:mm"), ResLateEx.ToString("HH:mm"), _
                             ResEarlyFor.ToString("HH:mm"), ResLateFor.ToString("HH:mm"), _
                             ResGameFlight, ResType, _
                             ResNotes}

    End Function

    Public Function FindIndexGFSByIDX_RL(ByVal _Group As GroupForSchedule) As Boolean
        Return _Group.IDX_RL = ResIDX_RL
    End Function

    Public Sub SetGroupScheduled()

        Dim tmp_row As Integer = 0

        tmp_row = m_GLO_GroupForScheduleList.FindIndex(AddressOf FindIndexGFSByIDX_RL)

        If tmp_row < 0 Then Exit Sub

        If m_GLO_GroupForScheduleList(tmp_row).Route.Count > 0 Then
            If ResIDX_FromAP = m_GLO_GroupForScheduleList(tmp_row).Route(0).IDX_FromAP _
                    And ResIDX_ToAP = m_GLO_GroupForScheduleList(tmp_row).Route(m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1).IDX_ToAP Then

                ResFlag = "OK"

                If (m_GLO_GroupForScheduleList(tmp_row).Route.Count) <= 1 Then
                    ResFlag = "OK"
                Else
                    For i As Integer = (m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1) To 1 Step -1
                        If m_GLO_GroupForScheduleList(tmp_row).Route(i).IDX_FromAP <> m_GLO_GroupForScheduleList(tmp_row).Route(i - 1).IDX_ToAP Then
                            ResFlag = "Fault"
                            Exit For
                        End If
                    Next i
                End If
            ElseIf ResIDX_FromAP = m_GLO_GroupForScheduleList(tmp_row).Route(0).IDX_FromAP _
                    And ResIDX_ToAP <> m_GLO_GroupForScheduleList(tmp_row).Route(m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1).IDX_ToAP Then

                ResFlag = "Dep OK"

                If (m_GLO_GroupForScheduleList(tmp_row).Route.Count) <= 1 Then
                    ResFlag = "Dep OK"
                Else
                    For i As Integer = (m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1) To 1 Step -1
                        If m_GLO_GroupForScheduleList(tmp_row).Route(i).IDX_FromAP <> m_GLO_GroupForScheduleList(tmp_row).Route(i - 1).IDX_ToAP Then
                            ResFlag = "Fault"
                            Exit For
                        End If
                    Next i
                End If
            ElseIf ResIDX_FromAP <> m_GLO_GroupForScheduleList(tmp_row).Route(0).IDX_FromAP _
                    And ResIDX_ToAP = m_GLO_GroupForScheduleList(tmp_row).Route(m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1).IDX_ToAP Then

                ResFlag = "Des OK"

                If (m_GLO_GroupForScheduleList(tmp_row).Route.Count) <= 1 Then
                    ResFlag = "Des OK"
                Else
                    For i As Integer = (m_GLO_GroupForScheduleList(tmp_row).Route.Count - 1) To 1 Step -1
                        If m_GLO_GroupForScheduleList(tmp_row).Route(i).IDX_FromAP <> m_GLO_GroupForScheduleList(tmp_row).Route(i - 1).IDX_ToAP Then
                            ResFlag = "Fault"
                            Exit For
                        End If
                    Next i
                End If
            Else
                ResFlag = "Fault"
            End If
        Else
            ResFlag = "Not Sched"
        End If
    End Sub

#End Region

#Region " System Defined Procedures "

    Public Sub New(ByVal _Selected As Boolean, ByVal _Flag As String, ByVal _FromAP As String,
                   ByVal _IDX_FromAP As Integer, ByVal _ToAP As String, ByVal _IDX_ToAP As Integer,
                   ByVal _Pax As Integer, ByVal _Name As String, ByVal _Ex As String,
                   ByVal _For As String, ByVal _Operator As String, ByVal _IDX_RH As Integer,
                   ByVal _IDX_RL As Integer, ByVal _Weight As Integer, ByVal _LuggageWeight As Integer,
                   ByVal _LastDest As String, ByVal _SoleUse As Boolean, ByVal _ACType As String,
                   ByVal _IDX_ACType As Integer, ByVal _EarlyEx As DateTime, ByVal _LateEx As DateTime,
                   ByVal _EarlyFor As DateTime, ByVal _LateFor As DateTime, ByVal _Cancelled As Boolean,
                   ByRef _Type As String, ByVal _Notes As String, ByVal _GameFlight As Integer, GLO_GroupForScheduleList As List(Of GroupForSchedule))

        m_ResSelected = _Selected
        m_ResFlag = _Flag
        m_ResFromAP = _FromAP
        m_ResIDX_FromAP = _IDX_FromAP
        m_ResToAP = _ToAP
        m_ResIDX_ToAP = _IDX_ToAP
        m_ResPax = _Pax
        m_ResName = _Name
        m_ResEx = _Ex
        m_ResFor = _For
        m_ResOperator = _Operator
        m_ResIDX_RH = _IDX_RH
        m_ResIDX_RL = _IDX_RL
        m_ResPassengerWeight = _Weight
        m_ResLuggageWeight = _LuggageWeight
        m_ResLastDest = _LastDest
        m_ResSoleUse = _SoleUse
        m_ResACType = _ACType
        m_ResIDX_ACType = _IDX_ACType
        m_ResEarlyEx = _EarlyEx
        m_ResLateEx = _LateEx
        m_ResEarlyFor = _EarlyFor
        m_ResLateFor = _LateFor
        m_ResCancelled = _Cancelled
        m_ResType = _Type
        m_ResNotes = _Notes
        m_ResGameFlight = _GameFlight
        m_GLO_GroupForScheduleList = GLO_GroupForScheduleList
        GLO_GroupForSchedule = New GroupForSchedule
        GLO_GroupForSchedule.IDX_RL = _IDX_RL
        GLO_GroupForSchedule.Reservation = _Name
        GLO_GroupForSchedule.SoleUse = _SoleUse
        GLO_GroupForSchedule.Route = New List(Of GroupScheduledRoute)

        GLO_GroupForScheduleList.Add(GLO_GroupForSchedule)

    End Sub

#End Region

End Class
