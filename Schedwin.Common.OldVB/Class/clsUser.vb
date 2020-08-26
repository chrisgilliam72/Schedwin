Option Explicit On

Imports System.Security.Principal

Public Class clsUser

    Dim m_Username As String
    Dim m_Password As String
    Dim m_Roles As Collection
    Dim m_IDXPersonnel As Integer
    Dim m_NumRoles As Integer
    Dim m_UserIdentity As GenericIdentity
    Dim m_UserPrincipal As GenericPrincipal

    Public Sub New()
        m_Username = ""
        m_Password = ""
        m_Roles = New Collection
        m_IDXPersonnel = 0
        m_NumRoles = 0       
    End Sub

    Friend Property Username() As String
        Get
            Username = m_Username
        End Get
        Set(ByVal Value As String)
            m_Username = Value
        End Set
    End Property

    Friend Property Password() As String
        Get
            Password = m_Password
        End Get
        Set(ByVal Value As String)
            m_Password = Value
        End Set
    End Property

    Friend Property IDXPersonnel() As Integer
        Get
            IDXPersonnel = m_IDXPersonnel
        End Get
        Set(ByVal Value As Integer)
            m_IDXPersonnel = Value
        End Set
    End Property

    Friend ReadOnly Property NumRoles() As Integer
        Get
            NumRoles = m_NumRoles
        End Get
    End Property

    Friend Function AddRole(ByVal Role As String, Optional ByVal index As Integer = 0) As Boolean
        m_Roles.Add(Role, CStr(index))
        m_NumRoles += 1
        Return True
    End Function

    Friend ReadOnly Property Roles(ByVal i As Integer) As String
        Get
            Roles = m_Roles(i)
        End Get
    End Property

    Friend Property UserIdentity() As GenericIdentity
        Get
            Return m_UserIdentity
        End Get
        Set(ByVal value As GenericIdentity)
            m_UserIdentity = value
        End Set
    End Property

    Friend Property UserPrincipal() As GenericPrincipal
        Get
            Return m_UserPrincipal
        End Get
        Set(ByVal value As GenericPrincipal)
            m_UserPrincipal = value
        End Set
    End Property



End Class