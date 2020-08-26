Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Security.Principal
Imports Microsoft.SqlServer.Management.Smo


Public Module GlobVariables

    Public GLO_Initialized As Boolean = False

#Region " Class "




#End Region ' Class

#Region " Constant "

    Public Const RateTypeComboList As String = "Seat|KM|Area|Minutes|Quote|Cur. Area|NAV Fees|DEP Tax|Landing Fees|Service|Cancel"
    Public Const DefaultColumnList As String = "0;NoList ; 0"
    Public Const DefaultColCombo As String = "0;NA;"

#End Region ' Constant

#Region " Dataset "

    Public ds As DataSet
    Public GLO_ds_OperatorsReservations As Data.DataSet
    Public GLO_ds_Pricelists As Data.DataSet
    Public GLO_ds_AircraftType As Data.DataSet
    Public GLO_ds_ExForFields As Data.DataSet
    Public GLO_ds_Airports As Data.DataSet

#End Region ' Dataset 



#Region " General "

    Public GLO_Office As String = "Default"
    Public GLO_GPConnectionInformation As GPConnectInformation
    Public GLO_GPServerFound As Boolean = False
    Public GLO_GPDatabaseFound As Boolean = False
    Public GLO_GPFound As Boolean = False
    Public GLO_DS_Updating As Boolean = False
    Public GLO_SQLTimeOut As Integer
    Public GLO_Access As New AccessScreens
    Public Logs As GLOSComponents.ErrorLogs
    Public Position As Find
    Public GLO_ResNewFormOpen As Boolean
    Public GLO_NewItem1 As String
    Public GLO_NewItem2 As String
    Public GLO_NewItem3 As String
    Public GLO_SetupInitialised As Boolean = False
    Public CurrSettings As SefoSystem
    Public VAR_user As WindowsIdentity = WindowsIdentity.GetCurrent
    'Public GLO_GroupForScheduleList As List(Of GroupForSchedule)
    'Public GLO_GroupForSchedule As GroupForSchedule

    Public GLO_SetupLoading As Boolean = False

    Public GLO_DistanceIDX As Collection
    Public GLO_AircraftAvionics As AircraftAvionics

#End Region ' General 

#Region " SQL "

    Public da As GLOSComponents.SQLConnect
    Public SQL_01Params(0) As SqlParameter
    Public SQL_02Params(1) As SqlParameter
    Public SQL_03Params(2) As SqlParameter
    Public SQL_04Params(3) As SqlParameter
    Public SQL_05Params(4) As SqlParameter
    Public SQL_06Params(5) As SqlParameter
    Public SQL_07Params(6) As SqlParameter
    Public SQL_08Params(7) As SqlParameter
    Public SQL_09Params(8) As SqlParameter
    Public SQL_10Params(9) As SqlParameter
    Public SQL_11Params(10) As SqlParameter
    Public SQL_12Params(11) As SqlParameter
    Public SQL_13Params(12) As SqlParameter
    Public SQL_16Params(15) As SqlParameter
    Public SQL_17Params(16) As SqlParameter
    Public SQL_25Params(24) As SqlParameter

#End Region ' SQL


End Module
