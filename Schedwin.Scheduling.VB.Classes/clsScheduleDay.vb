Public Class clsScheduleDay

    Private m_ScheduleACPilot As Collection
    Private m_ScheduleReservation As Collection
    Private m_IsLoaded As Boolean = False
    Private m_ScheduleNotes As String

    ''' <summary>
    ''' Creates new collection.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        m_ScheduleACPilot = New Collection
        m_ScheduleReservation = New Collection
        m_IsLoaded = False
    End Sub

    ''' <summary>
    ''' Returns the clsScheduleACPilot for the supplied collection index.
    ''' </summary>
    ''' <param name="i">Integer: Index number in the collection.</param>
    ''' <value></value>
    ''' <returns>clsScheduleACPilot: The information from the indexed collection.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScheduleACPilot(ByVal i As Integer) As clsScheduleACPilot
        Get
            If i > 0 Then
                ScheduleACPilot = m_ScheduleACPilot(i)
            Else
                Throw New Exception("Index out of bounds.")
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the complete collection under the clsScheduleDay
    ''' </summary>
    ''' <value></value>
    ''' <returns>Collection: The collection from clsScheduleDay</returns>
    ''' <remarks></remarks>
    Public Property ScheduleACPilot() As Collection
        Get
            ScheduleACPilot = m_ScheduleACPilot
        End Get
        Set(ByVal value As Collection)
            m_ScheduleACPilot = value
        End Set
    End Property

    Public ReadOnly Property ScheduleReservation(ByVal i As Integer) As clsScheduleReservations
        Get
            If i > 0 Then
                ScheduleReservation = m_ScheduleReservation(i)
            Else
                Throw New Exception("Index out of bounds.")
            End If
        End Get
    End Property

    Public Property ScheduleReservation() As Collection
        Get
            ScheduleReservation = m_ScheduleReservation
        End Get
        Set(ByVal value As Collection)
            m_ScheduleReservation = value
        End Set
    End Property

    Public Sub ClearReservations()
        m_ScheduleReservation = New Collection
    End Sub

    Public Sub ClearSchedule()
        m_ScheduleACPilot = New Collection
        m_ScheduleNotes = ""
    End Sub

    Public Property IsLoaded() As Boolean
        Get
            IsLoaded = m_IsLoaded
        End Get
        Set(ByVal value As Boolean)
            m_IsLoaded = value
        End Set
    End Property

    Public Function FindIndexOfGroup(ByVal _IDX_ResLeg As Integer) As Integer

        For i As Integer = 1 To ScheduleReservation.Count

            If ScheduleReservation(i).ResIDX_RL = _IDX_ResLeg Then Return i

        Next i

        Return -1

    End Function

End Class
