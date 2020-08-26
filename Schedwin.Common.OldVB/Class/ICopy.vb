''' <summary>
''' Interface for copying datatable into a collection.
''' </summary>
''' <remarks></remarks>
Public Interface ICopy
    Function NewCopy(ByVal params As Object(), ServerName As String, DatabaseName As String, Username As String, Password As String, GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy
End Interface

