import { Component } from "@angular/core";
import { Location } from "@angular/common";
import { PlatformService } from "../../services/platformService";
import { Router } from "@angular/router";
import { OAuthService } from "angular-oauth2-oidc";

@Component({
  selector: "app-header",
  templateUrl: "header.html",
  styleUrls: ["header.scss"]
})
export class HeaderComponent {
  public isLoggedIn = false;

  public get isBackChevronVisible(): boolean {
    return this._location.path() !== "/home" && this._platform.isIOS;
  }

  constructor(
    private _location: Location,
    private _router: Router,
    private _platform: PlatformService,
    private _security: OAuthService
  ) {
    this.isLoggedIn = this._security.hasValidAccessToken();
  }

  public logout(): void {
    this.isLoggedIn = false;
    this._security.logOut();
    this._router.navigate(["/home"]);
  }

  public goBack() {
    this._location.back();
  }
}
