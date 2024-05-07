namespace Facture.Core.Helpers
{
    public enum ErrorResponseCode
    {
        InternalError = 2000,
        InvalidInput = 2001,
        ArgumentsCauseError = 2002,
        MissingArguments = 2003,
        SinceOrUntilParametersInvalid = 2004,
        FailedAuthentication = 2005,
        TenantNotFound = 2006,
        BlockedTenant = 2007,
        OnlyHTTPSRequestAllow = 2008,
        AccessDenied = 2009,
        ParameterTenantIdRequired = 2011,
        ExpiredAccount = 2012
    }
}
