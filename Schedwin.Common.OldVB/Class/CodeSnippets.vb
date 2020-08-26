Imports System.Collections.Generic
Imports System.Drawing

Public Module GlobalDataStructures

#Region " Enumerators "
    Public Enum Numeric_Status
        Positive
        Negative
    End Enum
    Public Enum TechlogState
        Add
        Edit
        View
    End Enum


    Public Enum ReservationType
        Freight
        Passengers
        Staff
    End Enum

    Public Enum GPItemStatus
        Invoiced
        Pending
        Removed
    End Enum

    Public Enum SCH_Coordinates
        Longitude
        Latitude
    End Enum

    Public Enum FlightDirection
        ExFlight
        ForFlight
    End Enum

    Public Enum ResType
        Passenger
        Freight
        Staff
        Pilot
    End Enum

#End Region ' Enumerators

#Region " Structures "

    Public Structure GPConnectInformation
        Dim GP_Server As String
        Dim GP_Schedwin_Database As String
        Dim GP_FunctionalCurrency As String
        Dim GP_FunctionalCurrency_Desc As String
        Dim GP_FunctionalCurrency_Symbol As String
        Dim CURNCYID As String
        Dim GP_Username As String
        Dim GP_Password As String
    End Structure

    Public Structure CurrencyRate
        Dim Currnecy As String
        Dim Rate As Double
    End Structure

    Public Structure AircraftForSchedule
        Implements ICopy

        Dim IDX As Integer
        Dim Registration As String
        Dim IDX_ACAP As Integer
        Dim ACAP As String
        Dim IDX_ACType As Integer
        Dim ACType As String
        Dim ACService As Double
        Dim ACTypeWeight As Integer
        Dim ACBuyRate As Double
        Dim ACSellRate As Double
        Dim ACTypeRangeKM As Double
        Dim ACTypeRangeHours As Double
        Dim ACTypeSpeed As Integer
        Dim ACTypePax As Integer
        Dim ACTypeTurnaround As Integer
        Dim ACEmptyWeight As Double
        Dim ACTypeFuelFlow As Integer
        Dim ACReserveFuelWeight As Integer
        Dim IDX_FuelType As Integer
        Dim FuelType As String
        Dim ACOwn As Boolean

        Public Sub SetValues(ByVal _IDX As Integer,
                           ByVal _Registration As String,
                           ByVal _IDX_ACAP As Integer,
                           ByVal _ACAP As String,
                           ByVal _IDX_ACType As Integer,
                           ByVal _ACType As String,
                           ByVal _ACService As Double,
                           ByVal _ACTypeWeight As Integer,
                           ByVal _ACBuyRate As Double,
                           ByVal _ACSellRate As Double,
                           ByVal _ACTypeRangeKM As Double,
                           ByVal _ACTypeRangeHours As Double,
                           ByVal _ACTypeSpeed As Integer,
                           ByVal _ACTypePax As Integer,
                           ByVal _ACTypeTurnaround As Integer,
                           ByVal _ACEmptyWeight As Double,
                           ByVal _ACTypeFuelFlow As Integer,
                           ByVal _ACReserveFuelWeight As Integer,
                           ByVal _IDX_FuelType As Integer,
                           ByVal _FuelType As String,
                           ByVal _ACOwn As Boolean)

            IDX = _IDX
            Registration = _Registration
            IDX_ACAP = _IDX_ACAP
            ACAP = _ACAP
            IDX_ACType = _IDX_ACType
            ACType = _ACType
            ACService = _ACService
            ACTypeWeight = _ACTypeWeight
            ACBuyRate = _ACBuyRate
            ACSellRate = _ACSellRate
            ACTypeRangeKM = _ACTypeRangeKM
            ACTypeRangeHours = _ACTypeRangeHours
            ACTypeSpeed = _ACTypeSpeed
            ACTypePax = _ACTypePax
            ACTypeTurnaround = _ACTypeTurnaround
            ACEmptyWeight = _ACEmptyWeight
            ACTypeFuelFlow = _ACTypeFuelFlow
            ACReserveFuelWeight = _ACReserveFuelWeight
            IDX_FuelType = _IDX_FuelType
            FuelType = _FuelType
            ACOwn = _ACOwn

        End Sub

        Public Function NewCopy(ByVal params() As Object, ServerName As String, DatabaseName As String, Username As String, Password As String,
                                 GLO_GroupForScheduleList As List(Of GroupForSchedule)) As ICopy Implements ICopy.NewCopy

            SetValues(params(0), params(1), params(2), params(3), params(4), params(5) _
                                          , params(6), params(7), params(8), params(9), params(10) _
                                          , params(11), params(12), params(13), params(14), params(15) _
                                          , params(16), params(17), params(18), params(19), params(20))

        End Function

    End Structure

    Public Structure GroupForSchedule
        Dim IDX_RL As Integer
        Dim Reservation As String
        Dim SoleUse As Boolean
        Dim Route As List(Of GroupScheduledRoute)
    End Structure

    Public Structure GroupScheduledRoute
        Dim ETD As String
        Dim FromAP As String
        Dim IDX_FromAP As Integer
        Dim ETA As String
        Dim ToAP As String
        Dim IDX_ToAP As Integer
        Dim Aircraft As String

        Public Function RowObjects() As Object()
            Return New Object() {ETD, FromAP, ETA, ToAP, Aircraft}
        End Function

    End Structure

    Public Structure GroupToBeScheduled
        Dim IDX_RL As Integer
        Dim IDX_RH As Integer
        Dim ResPax As Integer
        Dim ResName As String
        Dim ResPaxWeight As Double
        Dim ResLuggageWeight As Double
        Dim EarlyEx As Date
        Dim EarlyFor As Date
        Dim LatestEx As Date
        Dim LatestFor As Date
        Dim SoleUse As Boolean
        Dim IDX_ACType As Integer
        Dim ACType As String
    End Structure

    Public Structure GroupToBeDeleted
        Dim IDX_ResLeg As Integer
        Dim IDX_SchLegRes As Integer
    End Structure

    Public Structure SefoSystem
        'Dim CountryID As Integer
        'Dim Companyname As String
        'Dim IDXCompany As Integer
        'Dim ServiceAP As String
        'Dim IDXServiceAP As Integer
        'Dim User As clsUser
        'Dim Currency As Integer
        'Dim DatabaseName As String
        'Dim Domain As String
        'Dim CoCode As String
        Dim VATPer As Integer
        Dim filterstr As String
        Dim filtercomp As String
        Dim filtercol As Short
        Dim defaultsort As Short '0: a->z; 1: z->a; -1:Not sorted
        Dim term1 As String
        Dim term2 As String
        Dim term3 As String
        Dim term4 As String
        Dim term5 As String
        Dim postscript As String
        Dim luggage As String
        Dim AdminEmailAddress As String
        Dim EmailDomain As String
        Dim SMTPServer As String
        Dim LogLocation As String
        Dim Auth As String
        Dim IsWISHEnabled As Boolean
        Dim WISHStatusRetrieve As String
        Dim IsTrackingEnabled As Boolean
        Dim PilotFlightTimeMultiplier As Double
        Dim DutyStartMinutes As Integer
        Dim DutyEndMinutes As Integer
        Dim Region As String
    End Structure

    Public Structure Fees
        Dim Name_1 As String
        Dim Name_2 As String
        Dim IDX_1 As String
        Dim IDX_2 As String
        Dim Parent As String
        Dim ParentIDX As String
        Dim Parent2 As String
        Dim ParentIDX2 As String
        Dim Parent3 As String
        Dim ParentIDX3 As String
        Dim Loaded As Boolean
        Dim IDXArray() As Integer
    End Structure

    Public Structure Find
        Dim IDX As Integer
        Dim IDXParent As Integer
        Dim IDXGrandParent As Integer
        Dim idxnode As Integer
    End Structure

    Public Structure ACImage
        Dim ImageStr As String
        Dim ImageID As Long
        Dim ImageChange As Boolean
        Dim ImageImg As Image
        Dim ImageThumb As Bitmap
    End Structure

    Public Structure MaintSched
        Dim ACType As Integer
        Dim ACExp As Boolean
        Dim Country As Integer
        Dim CountryExp As Boolean
        Dim MaintEvent As Integer
        Dim MaintExp As Boolean
    End Structure

    Public Structure AccessScreens
        Dim QuotesLevel As AccessLevel
        Dim ReportsLevel As AccessLevel
        Dim ServiceRecordsLevel As AccessLevel
        Dim SetupLevel As AccessLevel
        Dim SchedulingLevel As AccessLevel
        Dim ReservationsLevel As AccessLevel
        Dim WeightBalanceLevel As AccessLevel
        Dim TicketsLevel As AccessLevel
        Dim BaggageTagsLevel As AccessLevel
        Dim TechlogLevel As AccessLevel
        Dim ACPrepLevel As AccessLevel
        Dim LockSchedulesLevel As AccessLevel
        Dim WISHLevel As AccessLevel
        Dim GPLevel As AccessLevel
        Dim TrackingLevel As AccessLevel
    End Structure

    Public Structure AccessLevel
        Dim Access As Boolean
        Dim View As Boolean
        Dim Edit As Boolean
        Dim Add As Boolean
    End Structure

    Public Enum StationType
        Pax
        Fuel
        Aircraft
        Freight
    End Enum

    Public Structure AircraftAvionics
        Public NavComm1 As String
        Public NavComm2 As String
        Public Autopilot As String
        Public Wx As String
        Public GPS As String
        Public TCAS As String
        Public EGPWS As String
        Public HF As String
        Public FM As String
        Public MFD As String
        Public Transponder As String
        Public Other1 As String
        Public Other2 As String
        Public Other3 As String
        Public Other4 As String
        Public Other5 As String
    End Structure

#End Region ' Structures

End Module

