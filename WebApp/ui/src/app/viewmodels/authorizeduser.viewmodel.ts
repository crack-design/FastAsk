export class AuthorizedUserInfoViewModel {
  public userModel: AuthorizedUserModel;
  public token: string;
  public errorMessage: string;
}

class AuthorizedUserModel {
  public login: string;
  public id: number;
  public firstName: string;
  public lastName: string;
  public email: string;
}
