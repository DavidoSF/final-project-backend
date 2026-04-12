# Final Project Backend

## Appointments Backend Work

This backend contains the appointment management module for Person 3.

Implemented features:
- Appointment creation
- Appointment retrieval with optional filters
- Appointment retrieval by id
- Staff assignment and reassignment
- Status workflow management
- Validation of invalid status transitions
- Preservation of `PriceAtBooking` when an appointment is created

Status workflow:
- `Pending -> Approved`
- `Pending -> Rejected`
- `Approved -> Completed`
- `Approved -> Cancelled`

Final states:
- `Rejected`
- `Completed`
- `Cancelled`

Appointment API endpoints:
- `GET /api/appointments`
- `GET /api/appointments/{id}`
- `POST /api/appointments`
- `PUT /api/appointments/{id}/status`
- `PUT /api/appointments/{id}/assign-staff`
- `GET /api/appointments/staff-members`

Supported filters on `GET /api/appointments`:
- `status`
- `serviceId`
- `clientId`

Notes:
- Appointment statuses are returned as strings in JSON
- Invalid status transitions are rejected by the service layer
- Assignable users include active staff members and administrators
