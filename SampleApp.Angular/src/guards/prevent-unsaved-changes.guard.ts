import { CanDeactivateFn } from '@angular/router';
import { EditUserComponent } from '../app/edit-user/edit-user.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<EditUserComponent> = (component:EditUserComponent) => {

  if(component.editForm.dirty){
    return confirm("Вы хотите продолжить?")
  }

  return true;
};
