export interface User {
  Id: string;
  FullName: string;
  PasswordHash: string;
  Email: string;
  DateOfBirth?: Date;
  Region?: string;
  PhoneNumber?: string;
  ProfilePictureUrl?: string;
  ActiveStatus: string;
}
