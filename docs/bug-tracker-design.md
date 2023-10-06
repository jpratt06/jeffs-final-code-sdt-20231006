# Requirement 1


Software
User Identity
The date and time the bug was reported (UTC)
A short description of the issue
A narrative of the issue (being vague because who cares)


Result:

A link to the bug report
    - Should have information about the software, "slug"
    - The status of the bug, which is going to 
        "In Triage" (for now)



"Axes of Design"
    - Authority  server - where is this going to run.
    - Resources - "Nouns" - 
    - Representations - The data sent to or from the API
        - Body (entity) - Imanges/ Json, 
        - Route Params
        - Query String
        - Headers
            - Authorization - OIDC/Oauth2 - JWT
    - The HTTP Methods


    POST /catalog/excel/bugs
    Authorization: JWT
    {
       "description": "Excel goes boom",
       "narrative": "A big long thing with steps to reproduce"

    }


    201 Created
    Location: http://api.bugs.com/catalog/excel/bugs/excel-goes-boom

    {
        "id": "excel-goes-boom",
        "user": "Jeff",
        "software": "Excel"
        "issue": {
       "description": "Excel goes boom",
       "narrative": "A big long thing with steps to reproduce"

    },
    "status": "InTriage"

    }