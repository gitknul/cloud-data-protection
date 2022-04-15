export interface EmployeeResult {
    id: number;
    firstName: string;
    lastName: string;
    fullName: string;
    gender: Gender;
    contactEmailAddress: string;
    phoneNumber: string;
    createdAt: Date;
    activatedAt?: Date;
    activationStatus: EmployeeActivationStatus;
    activationStatusMessage: string;
    userId?: number;
    userEmailAddress: string;
}

export enum Gender {
    Male = 0,
    Female = 1,
    NotSpecified = 999
}

export enum EmployeeActivationStatus {
    PendingUserCreation = 0,
    PendingActivation = 1,
    Activated = 100,
    UserCreationFailed = 999,
    UserDeleted = 1000
}