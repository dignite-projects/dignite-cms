import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CmsComponent } from './components/cms.component';
import { CmsRoutingModule } from './cms-routing.module';

@NgModule({
  declarations: [CmsComponent],
  imports: [CoreModule, ThemeSharedModule, CmsRoutingModule],
  exports: [CmsComponent],
})
export class CmsModule {
  static forChild(): ModuleWithProviders<CmsModule> {
    return {
      ngModule: CmsModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<CmsModule> {
    return new LazyModuleFactory(CmsModule.forChild());
  }
}
