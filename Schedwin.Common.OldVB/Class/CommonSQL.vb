Imports System.Data
Imports System.Data.SqlClient
Imports ShowMessage

Public Class CommonSQL : Implements IDisposable

#Region " Variables "

    Private disposed As Boolean = False

    Private _Server As String
    Private _Database As String
    Private _UserID As String
    Private _Password As String
    Private _Timeout As Integer
    Private _SQL_Connection As SqlConnection
    Private _SQL_Transaction As SqlTransaction

    Public Enum TransactionState
        Begin
        Commit
        Roleback
    End Enum

    Public Enum ConnectionState
        Open
        Close
    End Enum

#End Region

#Region " Properties "

    Public ReadOnly Property Connection() As String
        Get
            Return "Data Source=" & Server & ";Initial Catalog=" & Database & ";User Id=" & UserID & ";Password=" & Password & ";Connection Timeout=" & Timeout & ";Integrated Security=SSPI;"
        End Get
    End Property

    Public Property Server() As String
        Get
            Return _Server
        End Get
        Set(ByVal value As String)
            _Server = value
        End Set
    End Property

    Public Property Database() As String
        Get
            Return _Database
        End Get
        Set(ByVal value As String)
            _Database = value
        End Set
    End Property

    Public Property UserID() As String
        Get
            Return _UserID
        End Get
        Set(ByVal value As String)
            _UserID = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public Property Timeout() As Integer
        Get
            Return _Timeout
        End Get
        Set(ByVal value As Integer)
            _Timeout = value
        End Set
    End Property

#End Region

#Region " System Defined "

    Public Sub New()

    End Sub

    Public Sub New(ByVal p_Server As String, ByVal p_Database As String, ByVal p_UserID As String, ByVal p_Password As String, ByVal p_Timeout As Integer)
        Server = p_Server
        Database = p_Database
        UserID = p_UserID
        Password = p_Password
        Timeout = p_Timeout
    End Sub

    ' IDisposable    
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                ''Free other state (managed objects). 

                ''Close and dispose sql transaction.
                If CheckObject(_SQL_Transaction) Then
                    _SQL_Transaction.Dispose()
                End If

                ''Close and dispose sql connection.
                If CheckObject(_SQL_Connection) Then
                    If (_SQL_Connection.State = Data.ConnectionState.Open) Then
                        _SQL_Connection.Close()
                    End If

                    _SQL_Connection.Dispose()
                End If

            End If
            ' Free your own state (unmanaged objects).          
            ' Set large fields to null.       
        End If
        Me.disposed = True
    End Sub

#Region " IDisposable Support "

    ' This code added by Visual Basic to    
    ' correctly implement the disposable pattern.    
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.       
        ' Put cleanup code in       
        ' Dispose(ByVal disposing As Boolean) above.       
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.       
        ' Put cleanup code in       
        ' Dispose(ByVal disposing As Boolean) above.       
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#End Region

#Region " User Defined "

    Public Sub SQLConnection(ByVal p_State As ConnectionState)

        Try

            Select Case p_State
                Case ConnectionState.Open
                    _SQL_Connection = New SqlConnection(Connection)
                    _SQL_Connection.Open()
                Case ConnectionState.Close

                    If _SQL_Connection.State = Data.ConnectionState.Open Then
                        _SQL_Connection.Close()
                    End If

                Case Else

            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub SQLTransaction(ByVal p_State As TransactionState)

        Try

            Select Case p_State
                Case TransactionState.Begin
                    _SQL_Transaction = _SQL_Connection.BeginTransaction
                Case TransactionState.Commit
                    _SQL_Transaction.Commit()
                Case TransactionState.Roleback
                    _SQL_Transaction.Rollback()
            End Select

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ExecCommand(ByVal p_command As String, ByVal p_commandtype As CommandType, ByRef r_datatset As DataSet)

        Try

            Using SQL_Command As New SqlCommand()

                Dim SQL_Adapter As SqlDataAdapter

                SQL_Command.Connection = _SQL_Connection
                SQL_Command.CommandText = p_command
                SQL_Command.CommandTimeout = _Timeout
                SQL_Command.CommandType = p_commandtype

                If CheckObject(_SQL_Transaction) Then
                    SQL_Command.Transaction = _SQL_Transaction
                End If

                SQL_Adapter = New SqlDataAdapter(SQL_Command)
                SQL_Adapter.Fill(r_datatset)

            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ExecCommand(ByVal p_command As String, ByVal p_commandtype As CommandType, ByRef r_integer As Integer)

        Try

            Using SQL_Command As New SqlCommand()

                SQL_Command.Connection = _SQL_Connection
                SQL_Command.CommandText = p_command
                SQL_Command.CommandTimeout = _Timeout
                SQL_Command.CommandType = p_commandtype

                If CheckObject(_SQL_Transaction) Then
                    SQL_Command.Transaction = _SQL_Transaction
                End If

                SQL_Command.ExecuteNonQuery()

                r_integer = 0

            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ExecCommand(ByVal p_command As String, ByVal p_commandtype As CommandType, ByRef r_datatset As DataSet, ByVal p_parameters() As SqlParameter)

        Try

            Using SQL_Command As New SqlCommand()

                Dim SQL_Adapter As SqlDataAdapter

                SQL_Command.Connection = _SQL_Connection
                SQL_Command.CommandText = p_command
                SQL_Command.CommandTimeout = _Timeout
                SQL_Command.CommandType = p_commandtype

                If CheckObject(_SQL_Transaction) Then
                    SQL_Command.Transaction = _SQL_Transaction
                End If

                For Each param As SqlParameter In p_parameters
                    SQL_Command.Parameters.Add(param)
                Next param

                SQL_Adapter = New SqlDataAdapter(SQL_Command)
                SQL_Adapter.Fill(r_datatset)

            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub ExecCommand(ByVal p_command As String, ByVal p_commandtype As CommandType, ByRef r_integer As Integer, ByVal p_parameters() As SqlParameter)

        Try

            Using SQL_Command As New SqlCommand()

                SQL_Command.Connection = _SQL_Connection
                SQL_Command.CommandText = p_command
                SQL_Command.CommandTimeout = _Timeout
                SQL_Command.CommandType = p_commandtype

                If CheckObject(_SQL_Transaction) Then
                    SQL_Command.Transaction = _SQL_Transaction
                End If

                For Each param As SqlParameter In p_parameters
                    SQL_Command.Parameters.Add(param)
                Next param

                SQL_Command.ExecuteNonQuery()

                r_integer = 0

            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region


End Class
