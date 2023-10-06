# Bug Tracker API

## Requirements

### Requirement 1 - Adding Bug Reports

Narrative: A logged in user should be able to use the API to report a bug in our software.

The report should include:

- The name of the software from our catalog of supported software.
- The name of the user submitting the report
- The date and time (UTC) that the bug was reported
- A short descriptive title of the bug
- A narrative describing the bug, steps to reproduce, etc.

Upon successfully reporting a bug, a new Bug Report should be created with the above information, and:

- A link to the bug report
    - the link should indicate the software the bug is related to, as well as a unique "slug" derived from the description of the bug
- A status of "InTriage"

### Requirement 2 - Retrieving Bug Reports

Given a user submits a bug as above, they should be able to check the status of the bug report by following the link generated above.
blah
