Создав и заполнив страницы Home и Help нашего учебного приложения, мы собираемся добавить страницу About. При совершении изменений такого характера хорошей привычкой является написание автоматизированных тестов, чтобы убедиться в правильной реализации какой-либо функции. Разрабатываемый в процессе построения приложения итоговый набор тестов будет служить в качестве защитной сетки и исполняемой документации для исходного кода приложения. Когда всё сделано правильно, написание тестов также позволяет нам разрабатывать быстрее, несмотря на необходимость написания дополнительного кода, так как в конечном счёте мы потратим меньше времени в попытках отследить ошибки. Это верно только в том случае, если вы мастер в написании тестов, и это ещё одна важная причина начать заниматься этим как можно раньше.

Особенно оживленная дискуссия идёт по поводу использования разработки через тестирование (test-driven development) (TDD) — техники тестирования, в который программист сначала пишет провальные тесты, а затем пишет код приложения для прохождения тестов.

## Когда тестировать

При принятии решения о том, когда и как тестировать, полезно понимать — зачем тестировать. На мой взгляд, написание автоматизированных тестов имеет три основных преимущества:

- Тесты защищают от регрессий, когда действовавшая ранее функция прекращает работать по какой-то причине.
- Тесты позволяют рефакторить (реорганизовать) код (т.е., изменять его форму без изменения функциональности) с бОльшей уверенностью.
- Тесты играют роль клиента для кода приложения, тем самым помогая определить его дизайн и взаимодействие с другими частями системы.
Хотя ни одно из указанных выше преимуществ не требует, чтобы тесты были написаны первыми, существует множество обстоятельств, когда разработка через тестирование (TDD) будет весьма ценным инструментом в вашем наборе. Решение, когда и как проверять, зависит отчасти и от того, насколько комфортно для вас написание тестов; многие разработчики считают, что как только они начинают лучше писать тесты, они становятся склонными начинать именно с них. Также оно зависит от того, насколько сложными являются тесты относительно кода приложения, насколько точно известна желаемая функциональность, и с какой вероятностью она в будущем может сломаться.

В этой ситуации, было бы полезным иметь набор рекомендаций о том, когда писать тесты первыми (или писать тесты вообще). Далее несколько советов, основанных на моём собственном опыте:

Когда тест особенно короткий или простой, относительно кода тестируемого приложения, склоняются к написанию теста первым.
Когда желаемое поведение не полностью понятно, склоняются к написанию кода приложения первым, а затем пишут тест для систематизации результата.
Поскольку безопасность является главным приоритетом, лучше сначала допустить ошибку при написании тестов модели безопасности.
Каждый раз, когда найдена ошибка, напишите тест для её воспроизведения и защиты от регрессии, затем напишите код приложения, чтобы исправить её.
Склоняюсь против написания тестов для кода (такого, как подробная HTML-структура), который может измениться в будущем.
Напишите тесты перед рефакторингом кода, сосредоточившись на тестировании подверженного ошибкам кода, который с большой вероятностью может быть нарушен.
На практике, вышеуказанные рекомендации означают, что обычно мы пишем первыми тесты моделей и контроллеров, а интеграционные тесты (тестируют функциональность через модели, представления и контроллеры) — вторыми. А когда мы пишем не слишком хрупкий или подверженный ошибкам код приложения, или код, который с большой вероятностью изменится (как это часто бывает с представлениями), мы часто пропускаем тестирование вообще.

Нашими главными инструментами тестирования будут тесты контроллеров (начнутся в этом разделе), тесты моделей и интеграционные тесты. Итеграционные тесты особенно мощные, поскольку они позволяют имитировать действия пользователя, взаимодействующего с приложением через браузер. Интеграционные тесты в конце концов будут нашей основной техникой тестирования, но тесты контроллера позволят нам более легко начать.

## Тестирование статусов ответов страниц

```Csharp
using System.Net;
using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using SampleApp.RazorPage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Sample.Test
{
    public class SampleTest
    {
        
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        public SampleTest()
        {
            // Arrange

            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task HomePage_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/Sign");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HelpPage_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/Auth");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
```

# Тестирование заголовков страниц

```
Install-Package HtmlAgilityPack
```


```Csharp
public class TitlePageTest
    {

        #region Конструктор
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public TitlePageTest()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }
        #endregion

        [Fact]
        public async Task Get_Home_ReturnsSuccess_WithCorrectTitle()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/Sign");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
            var title = System.Net.WebUtility.HtmlDecode(titleNode.InnerHtml);
            Assert.NotNull(titleNode);
            Assert.Equal("Регистрация", title);
        }

        [Fact]
        public async Task Get_Help_ReturnsSuccess_WithCorrectTitle()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/Auth");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
            var title = System.Net.WebUtility.HtmlDecode(titleNode.InnerHtml);
            Assert.NotNull(titleNode);
            Assert.Equal("Авторизация", title);
        }

        [Fact]
        public async Task Get_About_ReturnsSuccess_WithCorrectTitle()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/About");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
            var title = System.Net.WebUtility.HtmlDecode(titleNode.InnerHtml);
            Assert.NotNull(titleNode);
            Assert.Equal("About", title);
        }
    }
```

**Задание**. В интеграционных тестах добавьте код для посещения страницы регистрации с помощью метода get и убедитесь, что заголовок полученной страницы является верным.


# Тесты Ruby on Rails

Тест для поимки нежелательной продолжительности флэша

```Ruby
require 'test_helper'

class UsersLoginTest < ActionDispatch::IntegrationTest

  test "login with invalid information" do
    get login_path
    assert_template 'sessions/new'
    post login_path, session: { email: "", password: "" }
    assert_template 'sessions/new'
    assert_not flash.empty?
    get root_path
    assert flash.empty?
  end
end
```

## Тест для входа пользователя с валидной информацией.

```Ruby
require 'test_helper'

class UsersLoginTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end
  .
  .
  .
  test "login with valid information" do
    get login_path
    post login_path, session: { email: @user.email, password: 'password' }
    assert_redirected_to @user
    follow_redirect!
    assert_template 'users/show'
    assert_select "a[href=?]", login_path, count: 0
    assert_select "a[href=?]", logout_path
    assert_select "a[href=?]", user_path(@user)
  end
end
```

##  Тест входа после регистрации

```Ruby
require 'test_helper'

class UsersSignupTest < ActionDispatch::IntegrationTest
  .
  .
  .
  test "valid signup information" do
    get signup_path
    assert_difference 'User.count', 1 do
      post_via_redirect users_path, user: { name:  "Example User",
                                            email: "user@example.com",
                                            password:              "password",
                                            password_confirmation: "password" }
    end
    assert_template 'users/show'
    assert is_logged_in?
  end
end
```

##  Тест выхода пользователя

```Ruby
require 'test_helper'

class UsersLoginTest < ActionDispatch::IntegrationTest
  .
  .
  .
  test "login with valid information followed by logout" do
    get login_path
    post login_path, session: { email: @user.email, password: 'password' }
    assert is_logged_in?
    assert_redirected_to @user
    follow_redirect!
    assert_template 'users/show'
    assert_select "a[href=?]", login_path, count: 0
    assert_select "a[href=?]", logout_path
    assert_select "a[href=?]", user_path(@user)
    delete logout_path
    assert_not is_logged_in?
    assert_redirected_to root_url
    follow_redirect!
    assert_select "a[href=?]", login_path
    assert_select "a[href=?]", logout_path,      count: 0
    assert_select "a[href=?]", user_path(@user), count: 0
  end
end
```

## Тест для функции запоминания

```Ruby
require 'test_helper'

class UsersLoginTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end
  .
  .
  .
  test "login with remembering" do
    log_in_as(@user, remember_me: '1')
    assert_equal FILL_IN, assigns(:user).FILL_IN
  end

  test "login without remembering" do
    log_in_as(@user, remember_me: '0')
    assert_nil cookies['remember_token']
  end
  .
  .
  .
end
```

## Тест для провального редактирования

```Ruby
require 'test_helper'

class UsersEditTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end

  test "unsuccessful edit" do
    get edit_user_path(@user)
    assert_template 'users/edit'
    patch user_path(@user), user: { name:  "",
                                    email: "foo@invalid",
                                    password:              "foo",
                                    password_confirmation: "bar" }
    assert_template 'users/edit'
  end
end
```

## Тест успешного редактирования

```Ruby
require 'test_helper'

class UsersEditTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end
  .
  .
  .
  test "successful edit" do
    get edit_user_path(@user)
    assert_template 'users/edit'
    name  = "Foo Bar"
    email = "foo@bar.com"
    patch user_path(@user), user: { name:  name,
                                    email: email,
                                    password:              "",
                                    password_confirmation: "" }
    assert_not flash.empty?
    assert_redirected_to @user
    @user.reload
    assert_equal name,  @user.name
    assert_equal email, @user.email
  end
end
```

## Осуществление входа тестового пользователя

```Ruby
require 'test_helper'

class UsersEditTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end

  test "unsuccessful edit" do
    log_in_as(@user)
    get edit_user_path(@user)
    .
    .
    .
  end

  test "successful edit" do
    log_in_as(@user)
    get edit_user_path(@user)
    .
    .
    .
  end
end
```

## Тестирование того, что edit и update защищены

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user = users(:michael)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should redirect edit when not logged in" do
    get :edit, id: @user
    assert_not flash.empty?
    assert_redirected_to login_url
  end

  test "should redirect update when not logged in" do
    patch :update, id: @user, user: { name: @user.name, email: @user.email }
    assert_not flash.empty?
    assert_redirected_to login_url
  end
end
```

## Тесты попытки редактирования не того пользователя

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user       = users(:michael)
    @other_user = users(:archer)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should redirect edit when not logged in" do
    get :edit, id: @user
    assert_not flash.empty?
    assert_redirected_to login_url
  end

  test "should redirect update when not logged in" do
    patch :update, id: @user, user: { name: @user.name, email: @user.email }
    assert_not flash.empty?
    assert_redirected_to login_url
  end

  test "should redirect edit when logged in as wrong user" do
    log_in_as(@other_user)
    get :edit, id: @user
    assert flash.empty?
    assert_redirected_to root_url
  end

  test "should redirect update when logged in as wrong user" do
    log_in_as(@other_user)
    patch :update, id: @user, user: { name: @user.name, email: @user.email }
    assert flash.empty?
    assert_redirected_to root_url
  end
end
```

## Тест дружелюбной переадресации

```Ruby
require 'test_helper'

class UsersEditTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end
  .
  .
  .
  test "successful edit with friendly forwarding" do
    get edit_user_path(@user)
    log_in_as(@user)
    assert_redirected_to edit_user_path(@user)
    name  = "Foo Bar"
    email = "foo@bar.com"
    patch user_path(@user), user: { name:  name,
                                    email: email,
                                    password:              "",
                                    password_confirmation: "" }
    assert_not flash.empty?
    assert_redirected_to @user
    @user.reload
    assert_equal name,  @user.name
    assert_equal email, @user.email
  end
end
```

## Тестирование перенаправления действия index

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user       = users(:michael)
    @other_user = users(:archer)
  end

  test "should redirect index when not logged in" do
    get :index
    assert_redirected_to login_url
  end
  .
  .
  .
end
```

## Тест для списка пользователей, в том числе пагинации

```Ruby
require 'test_helper'

class UsersIndexTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end

  test "index including pagination" do
    log_in_as(@user)
    get users_path
    assert_template 'users/index'
    assert_select 'div.pagination'
    User.paginate(page: 1).each do |user|
      assert_select 'a[href=?]', user_path(user), text: user.name
    end
  end
end
```

## Тесты на уровне действия для контроля доступа

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user       = users(:michael)
    @other_user = users(:archer)
  end
  .
  .
  .
  test "should redirect destroy when not logged in" do
    assert_no_difference 'User.count' do
      delete :destroy, id: @user
    end
    assert_redirected_to login_url
  end

  test "should redirect destroy when logged in as a non-admin" do
    log_in_as(@other_user)
    assert_no_difference 'User.count' do
      delete :destroy, id: @user
    end
    assert_redirected_to root_url
  end
end
```

## Интеграционные тесты для ссылок на удаление и собственно удаления пользователей

```Ruby
require 'test_helper'

class UsersIndexTest < ActionDispatch::IntegrationTest

  def setup
    @admin     = users(:michael)
    @non_admin = users(:archer)
  end

  test "index as admin including pagination and delete links" do
    log_in_as(@admin)
    get users_path
    assert_template 'users/index'
    assert_select 'div.pagination'
    first_page_of_users = User.paginate(page: 1)
    first_page_of_users.each do |user|
      assert_select 'a[href=?]', user_path(user), text: user.name
      unless user == @admin
        assert_select 'a[href=?]', user_path(user), text: 'delete'
      end
    end
    assert_difference 'User.count', -1 do
      delete user_path(@non_admin)
    end
  end

  test "index as non-admin" do
    log_in_as(@non_admin)
    get users_path
    assert_select 'a', text: 'delete', count: 0
  end
end
```

## Тестирование того, что атрибут admin не доступен для редактирования

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user       = users(:michael)
    @other_user = users(:archer)
  end
  .
  .
  .
  test "should redirect update when logged in as wrong user" do
    log_in_as(@other_user)
    patch :update, id: @user, user: { name: @user.name, email: @user.email }
    assert_redirected_to root_url
  end

  test "should not allow the admin attribute to be edited via the web" do
    log_in_as(@other_user)
    assert_not @other_user.admin?
    patch :update, id: @other_user, user: { password:              FILL_IN,
                                            password_confirmation: FILL_IN,
                                            admin: FILL_IN }
    assert_not @other_user.FILL_IN.admin?
  end
  .
  .
  .
end
```

## Тесты мэйлера User, сгенерированные Rails.

```Ruby
require 'test_helper'

class UserMailerTest < ActionMailer::TestCase

  test "account_activation" do
    mail = UserMailer.account_activation
    assert_equal "Account activation", mail.subject
    assert_equal ["to@example.org"], mail.to
    assert_equal ["from@example.com"], mail.from
    assert_match "Hi", mail.body.encoded
  end

  test "password_reset" do
    mail = UserMailer.password_reset
    assert_equal "Password reset", mail.subject
    assert_equal ["to@example.org"], mail.to
    assert_equal ["from@example.com"], mail.from
    assert_match "Hi", mail.body.encoded
  end
end
```

## Тест текущей реализации электронного письма

```Ruby
require 'test_helper'

class UserMailerTest < ActionMailer::TestCase

  test "account_activation" do
    user = users(:michael)
    user.activation_token = User.new_token
    mail = UserMailer.account_activation(user)
    assert_equal "Account activation", mail.subject
    assert_equal [user.email], mail.to
    assert_equal ["noreply@example.com"], mail.from
    assert_match user.name,               mail.body.encoded
    assert_match user.activation_token,   mail.body.encoded
    assert_match CGI::escape(user.email), mail.body.encoded
  end
end
```

## Добавление активации аккаунта к тестам регистрации пользователя

```Ruby
require 'test_helper'

class UsersSignupTest < ActionDispatch::IntegrationTest

  def setup
    ActionMailer::Base.deliveries.clear
  end

  test "invalid signup information" do
    get signup_path
    assert_no_difference 'User.count' do
      post users_path, user: { name:  "",
                               email: "user@invalid",
                               password:              "foo",
                               password_confirmation: "bar" }
    end
    assert_template 'users/new'
    assert_select 'div#error_explanation'
    assert_select 'div.field_with_errors'
  end

  test "valid signup information with account activation" do
    get signup_path
    assert_difference 'User.count', 1 do
      post users_path, user: { name:  "Example User",
                               email: "user@example.com",
                               password:              "password",
                               password_confirmation: "password" }
    end
    assert_equal 1, ActionMailer::Base.deliveries.size
    user = assigns(:user)
    assert_not user.activated?
    # Попытка войти до активации
    log_in_as(user)
    assert_not is_logged_in?
    # Невалидный активационный токен
    get edit_account_activation_path("invalid token")
    assert_not is_logged_in?
    # Валидный токен, неверный адрес электронной почты
    get edit_account_activation_path(user.activation_token, email: 'wrong')
    assert_not is_logged_in?
    # Валидный активационный токен и адрес почты
    get edit_account_activation_path(user.activation_token, email: user.email)
    assert user.reload.activated?
    follow_redirect!
    assert_template 'users/show'
    assert is_logged_in?
  end
end
```

## Добавление тестов метода мэйлера для сброса пароля

```Ruby
require 'test_helper'

class UserMailerTest < ActionMailer::TestCase

  test "account_activation" do
    user = users(:michael)
    user.activation_token = User.new_token
    mail = UserMailer.account_activation(user)
    assert_equal "Account activation", mail.subject
    assert_equal [user.email], mail.to
    assert_equal ["noreply@example.com"], mail.from
    assert_match user.name,               mail.body.encoded
    assert_match user.activation_token,   mail.body.encoded
    assert_match CGI::escape(user.email), mail.body.encoded
  end

  test "password_reset" do
    user = users(:michael)
    user.reset_token = User.new_token
    mail = UserMailer.password_reset(user)
    assert_equal "Password reset", mail.subject
    assert_equal [user.email], mail.to
    assert_equal ["noreply@example.com"], mail.from
    assert_match user.reset_token,        mail.body.encoded
    assert_match CGI::escape(user.email), mail.body.encoded
  end
end
```

## Интеграционные тесты для сброса пароля.

```Ruby
require 'test_helper'

class PasswordResetsTest < ActionDispatch::IntegrationTest

  def setup
    ActionMailer::Base.deliveries.clear
    @user = users(:michael)
  end

  test "password resets" do
    get new_password_reset_path
    assert_template 'password_resets/new'
    # Неверный адрес электронной почты
    post password_resets_path, password_reset: { email: "" }
    assert_not flash.empty?
    assert_template 'password_resets/new'
    # Правильный адрес электронной почты
    post password_resets_path, password_reset: { email: @user.email }
    assert_not_equal @user.reset_digest, @user.reload.reset_digest
    assert_equal 1, ActionMailer::Base.deliveries.size
    assert_not flash.empty?
    assert_redirected_to root_url
    # Форма смены пароля
    user = assigns(:user)
    # Неверный email
    get edit_password_reset_path(user.reset_token, email: "")
    assert_redirected_to root_url
    # Неактивный пользователь
    user.toggle!(:activated)
    get edit_password_reset_path(user.reset_token, email: user.email)
    assert_redirected_to root_url
    user.toggle!(:activated)
    # Верный email, неправильный токен
    get edit_password_reset_path('wrong token', email: user.email)
    assert_redirected_to root_url
    # Верный email, правильный токен
    get edit_password_reset_path(user.reset_token, email: user.email)
    assert_template 'password_resets/edit'
    assert_select "input[name=email][type=hidden][value=?]", user.email
    # Невалидный пароль & подтверждение
    patch password_reset_path(user.reset_token),
          email: user.email,
          user: { password:              "foobaz",
                  password_confirmation: "barquux" }
    assert_select 'div#error_explanation'
    # Пустой пароль
    patch password_reset_path(user.reset_token),
          email: user.email,
          user: { password:              "",
                  password_confirmation: "" }
    assert_select 'div#error_explanation'
    # Валидный пароль & подтверждение
    patch password_reset_path(user.reset_token),
          email: user.email,
          user: { password:              "foobaz",
                  password_confirmation: "foobaz" }
    assert is_logged_in?
    assert_not flash.empty?
    assert_redirected_to user
  end
end
```

## Тест срока годности ссылки на сброс пароля

```Ruby
require 'test_helper'

class PasswordResetsTest < ActionDispatch::IntegrationTest

  def setup
    ActionMailer::Base.deliveries.clear
    @user = users(:michael)
  end
  .
  .
  .
  test "expired token" do
    get new_password_reset_path
    post password_resets_path, password_reset: { email: @user.email }

    @user = assigns(:user)
    @user.update_attribute(:reset_sent_at, 3.hours.ago)
    patch password_reset_path(@user.reset_token),
          email: @user.email,
          user: { password:              "foobar",
                  password_confirmation: "foobar" }
    assert_response :redirect
    follow_redirect!
    assert_match /FILL_IN/i, response.body
  end
end
```

## Тесты валидности нового микросообщения

```Ruby
require 'test_helper'

class MicropostTest < ActiveSupport::TestCase

  def setup
    @user = users(:michael)
    # Этот код идиоматически не корректен.
    @micropost = Micropost.new(content: "Lorem ipsum", user_id: @user.id)
  end

  test "should be valid" do
    assert @micropost.valid?
  end

  test "user id should be present" do
    @micropost.user_id = nil
    assert_not @micropost.valid?
  end
end
```

## Тесты для валидаций модели Micropost

```Ruby
require 'test_helper'

class MicropostTest < ActiveSupport::TestCase

  def setup
    @user = users(:michael)
    @micropost = Micropost.new(content: "Lorem ipsum", user_id: @user.id)
  end

  test "should be valid" do
    assert @micropost.valid?
  end

  test "user id should be present" do
    @micropost.user_id = nil
    assert_not @micropost.valid?
  end

  test "content should be present" do
    @micropost.content = "   "
    assert_not @micropost.valid?
  end

  test "content should be at most 140 characters" do
    @micropost.content = "a" * 141
    assert_not @micropost.valid?
  end
end
```

## Идиоматически верный код для построения микроособщения

```Ruby
require 'test_helper'

class MicropostTest < ActiveSupport::TestCase

  def setup
    @user = users(:michael)
    @micropost = @user.microposts.build(content: "Lorem ipsum")
  end

  test "should be valid" do
    assert @micropost.valid?
  end

  test "user id should be present" do
    @micropost.user_id = nil
    assert_not @micropost.valid?
  end
  .
  .
  .
end
```

## Тестирование порядка микросообщений

```Ruby
require 'test_helper'

class MicropostTest < ActiveSupport::TestCase
  .
  .
  .
  test "order should be most recent first" do
    assert_equal microposts(:most_recent), Micropost.first
  end
end
```

## Тест dependent: :destroy.

```Ruby
require 'test_helper'

class UserTest < ActiveSupport::TestCase

  def setup
    @user = User.new(name: "Example User", email: "user@example.com",
                     password: "foobar", password_confirmation: "foobar")
  end
  .
  .
  .
  test "associated microposts should be destroyed" do
    @user.save
    @user.microposts.create!(content: "Lorem ipsum")
    assert_difference 'Micropost.count', -1 do
      @user.destroy
    end
  end
end
```

## Тест профиля пользователя

```Ruby
require 'test_helper'

class UsersProfileTest < ActionDispatch::IntegrationTest
  include ApplicationHelper

  def setup
    @user = users(:michael)
  end

  test "profile display" do
    get user_path(@user)
    assert_template 'users/show'
    assert_select 'title', full_title(@user.name)
    assert_select 'h1', text: @user.name
    assert_select 'h1>img.gravatar'
    assert_match @user.microposts.count.to_s, response.body
    assert_select 'div.pagination'
    @user.microposts.paginate(page: 1).each do |micropost|
      assert_match micropost.content, response.body
    end
  end
end
```

## Авторизационые тесты для контроллера Microposts

```Ruby
require 'test_helper'

class MicropostsControllerTest < ActionController::TestCase

  def setup
    @micropost = microposts(:orange)
  end

  test "should redirect create when not logged in" do
    assert_no_difference 'Micropost.count' do
      post :create, micropost: { content: "Lorem ipsum" }
    end
    assert_redirected_to login_url
  end

  test "should redirect destroy when not logged in" do
    assert_no_difference 'Micropost.count' do
      delete :destroy, id: @micropost
    end
    assert_redirected_to login_url
  end
end
```

## Тестирование удаления микросообщений несоответствующим пользователем

```Ruby
require 'test_helper'

class MicropostsControllerTest < ActionController::TestCase

  def setup
    @micropost = microposts(:orange)
  end

  test "should redirect create when not logged in" do
    assert_no_difference 'Micropost.count' do
      post :create, micropost: { content: "Lorem ipsum" }
    end
    assert_redirected_to login_url
  end

  test "should redirect destroy when not logged in" do
    assert_no_difference 'Micropost.count' do
      delete :destroy, id: @micropost
    end
    assert_redirected_to login_url
  end

  test "should redirect destroy for wrong micropost" do
    log_in_as(users(:michael))
    micropost = microposts(:ants)
    assert_no_difference 'Micropost.count' do
      delete :destroy, id: micropost
    end
    assert_redirected_to root_url
  end
end
```

## Интеграционный тест для интерфейса микросообщений

```Ruby
require 'test_helper'

class MicropostsInterfaceTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end

  test "micropost interface" do
    log_in_as(@user)
    get root_path
    assert_select 'div.pagination'
    # Невалидная отправка формы.
    assert_no_difference 'Micropost.count' do
      post microposts_path, micropost: { content: "" }
    end
    assert_select 'div#error_explanation'
    # Валидная отправка формы.
    content = "This micropost really ties the room together"
    assert_difference 'Micropost.count', 1 do
      post microposts_path, micropost: { content: content }
    end
    assert_redirected_to root_url
    follow_redirect!
    assert_match content, response.body
    # Удаление сообщения.
    assert_select 'a', text: 'delete'
    first_micropost = @user.microposts.paginate(page: 1).first
    assert_difference 'Micropost.count', -1 do
      delete micropost_path(first_micropost)
    end
    # Посещение профиля другого пользователя.
    get user_path(users(:archer))
    assert_select 'a', text: 'delete', count: 0
  end
end
```

## Шаблон для теста счетчика микросообщений на боковой панели

```Ruby
require 'test_helper'

class MicropostInterfaceTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end
  .
  .
  .
  test "micropost sidebar count" do
    log_in_as(@user)
    get root_path
    assert_match "#{FILL_IN} microposts", response.body
    # Пользователь без микросообщений
    other_user = users(:malory)
    log_in_as(other_user)
    get root_path
    assert_match "0 microposts", response.body
    other_user.microposts.create!(content: "A micropost")
    get root_path
    assert_match FILL_IN, response.body
  end
end
```

## Шаблон для тестирования загрузки изображений

```Ruby
require 'test_helper'

class MicropostInterfaceTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
  end

  test "micropost interface" do
    log_in_as(@user)
    get root_path
    assert_select 'div.pagination'
    assert_select 'input[type=FILL_IN]'
    # Невалидная отправка формы.
    post microposts_path, micropost: { content: "" }
    assert_select 'div#error_explanation'
    # Валидная отправка формы.
    content = "This micropost really ties the room together"
    picture = fixture_file_upload('test/fixtures/rails.png', 'image/png')
    assert_difference 'Micropost.count', 1 do
      post microposts_path, micropost: { content: content, picture: FILL_IN }
    end
    assert FILL_IN.picture?
    follow_redirect!
    assert_match content, response.body
    # Удаление сообщения.
    assert_select 'a', 'delete'
    first_micropost = @user.microposts.paginate(page: 1).first
    assert_difference 'Micropost.count', -1 do
      delete micropost_path(first_micropost)
    end
    # Посещение профиля другого пользователся.
    get user_path(users(:archer))
    assert_select 'a', { text: 'delete', count: 0 }
  end
  .
  .
  .
end
```

## Тестирование валидаций модели Relationship

```Ruby
require 'test_helper'

class RelationshipTest < ActiveSupport::TestCase

  def setup
    @relationship = Relationship.new(follower_id: 1, followed_id: 2)
  end

  test "should be valid" do
    assert @relationship.valid?
  end

  test "should require a follower_id" do
    @relationship.follower_id = nil
    assert_not @relationship.valid?
  end

  test "should require a followed_id" do
    @relationship.followed_id = nil
    assert_not @relationship.valid?
  end
end
```

##  Тесты “читательных” служебных методов

```Ruby
require 'test_helper'

class UserTest < ActiveSupport::TestCase
  .
  .
  .
  test "should follow and unfollow a user" do
    michael = users(:michael)
    archer  = users(:archer)
    assert_not michael.following?(archer)
    michael.follow(archer)
    assert michael.following?(archer)
    michael.unfollow(archer)
    assert_not michael.following?(archer)
  end
end
```

## Тест читателей

```Ruby
require 'test_helper'

class UserTest < ActiveSupport::TestCase
  .
  .
  .
  test "should follow and unfollow a user" do
    michael  = users(:michael)
    archer   = users(:archer)
    assert_not michael.following?(archer)
    michael.follow(archer)
    assert michael.following?(archer)
    assert archer.followers.include?(michael)
    michael.unfollow(archer)
    assert_not michael.following?(archer)
  end
end
```

## Тесты авторизации для страниц со списком читаемых и читателей

```Ruby
require 'test_helper'

class UsersControllerTest < ActionController::TestCase

  def setup
    @user = users(:michael)
    @other_user = users(:archer)
  end
  .
  .
  .
  test "should redirect following when not logged in" do
    get :following, id: @user
    assert_redirected_to login_url
  end

  test "should redirect followers when not logged in" do
    get :followers, id: @user
    assert_redirected_to login_url
  end
end
```

## Тесты для страниц “читаемые/читатели”

```Ruby
require 'test_helper'

class FollowingTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
    log_in_as(@user)
  end

  test "following page" do
    get following_user_path(@user)
    assert_not @user.following.empty?
    assert_match @user.following.count.to_s, response.body
    @user.following.each do |user|
      assert_select "a[href=?]", user_path(user)
    end
  end

  test "followers page" do
    get followers_user_path(@user)
    assert_not @user.followers.empty?
    assert_match @user.followers.count.to_s, response.body
    @user.followers.each do |user|
      assert_select "a[href=?]", user_path(user)
    end
  end
end
```

## Основные тесты контроля доступа для взаимоотношений

```Ruby
require 'test_helper'

class RelationshipsControllerTest < ActionController::TestCase

  test "create should require logged-in user" do
    assert_no_difference 'Relationship.count' do
      post :create
    end
    assert_redirected_to login_url
  end

  test "destroy should require logged-in user" do
    assert_no_difference 'Relationship.count' do
      delete :destroy, id: relationships(:one)
    end
    assert_redirected_to login_url
  end
end
```

## Тесты для кнопок “Читать” и “Не читать”

```Ruby
require 'test_helper'

class FollowingTest < ActionDispatch::IntegrationTest

  def setup
    @user  = users(:michael)
    @other = users(:archer)
    log_in_as(@user)
  end
  .
  .
  .
  test "should follow a user the standard way" do
    assert_difference '@user.following.count', 1 do
      post relationships_path, followed_id: @other.id
    end
  end

  test "should follow a user with Ajax" do
    assert_difference '@user.following.count', 1 do
      xhr :post, relationships_path, followed_id: @other.id
    end
  end

  test "should unfollow a user the standard way" do
    @user.follow(@other)
    relationship = @user.active_relationships.find_by(followed_id: @other.id)
    assert_difference '@user.following.count', -1 do
      delete relationship_path(relationship)
    end
  end

  test "should unfollow a user with Ajax" do
    @user.follow(@other)
    relationship = @user.active_relationships.find_by(followed_id: @other.id)
    assert_difference '@user.following.count', -1 do
      xhr :delete, relationship_path(relationship)
    end
  end
end
```

## Тесты ленты сообщений

```Ruby
require 'test_helper'

class UserTest < ActiveSupport::TestCase
  .
  .
  .
  test "feed should have the right posts" do
    michael = users(:michael)
    archer  = users(:archer)
    lana    = users(:lana)
    # Сообщения читаемого пользователя.
    lana.microposts.each do |post_following|
      assert michael.feed.include?(post_following)
    end
    # Собственные сообщения.
    michael.microposts.each do |post_self|
      assert michael.feed.include?(post_self)
    end
    # Сообщения нечитаемого пользователя.
    archer.microposts.each do |post_unfollowed|
      assert_not michael.feed.include?(post_unfollowed)
    end
  end
end
```

## Тестирование HTML потока сообщений

```Ruby
require 'test_helper'

class FollowingTest < ActionDispatch::IntegrationTest

  def setup
    @user = users(:michael)
    log_in_as(@user)
  end
  .
  .
  .
  test "feed on Home page" do
    get root_path
    @user.feed.paginate(page: 1).each do |micropost|
      assert_match CGI.escapeHTML(FILL_IN), FILL_IN
    end
  end
end
```


# xUnit

## Тестирование условий отказа

```Csharp
[Fact]
public void ThrowsExceptionIfRateIsZero()
{
 var converter = new CurrencyConverter();
 const decimal value = 1;
 const decimal rate = 0;
 const int dp = 2;
 var ex = Assert.Throws<ArgumentException>(
 () => converter.ConvertToGbp(value, rate, dp));
 // Дальнейшие утверждения относительно сгенерированного исключения, ex.
}
```

## Тестирование промежуточного ПО - middleware

```Csharp
[Fact]
public async Task ForNonMatchingRequest_CallsNextDelegate()
{
 var context = new DefaultHttpContext();
 context.Request.Path = "/somethingelse";
var wasExecuted = false;
 RequestDelegate next = (HttpContext ctx) =>
 {
 wasExecuted = true;
 return Task.CompletedTask;
 };
 var middleware = new StatusMiddleware(next);
 await middleware.Invoke(context);
 Assert.True(wasExecuted);
}
```


```Csharp
[Fact]
public async Task ReturnsPongBodyContent()
{
 var bodyStream = new MemoryStream();
 var context = new DefaultHttpContext();
 context.Response.Body = bodyStream;
 context.Request.Path = "/ping";
 RequestDelegate next = (ctx) => Task.CompletedTask;
 var middleware = new StatusMiddleware(next: next);
await middleware.Invoke(context);
 string response;
 bodyStream.Seek(0, SeekOrigin.Begin);
 using (var stringReader = new StreamReader(bodyStream))
 {
 response = await stringReader.ReadToEndAsync();
 }
 Assert.Equal("pong", response);
 Assert.Equal("text/plain", context.Response.ContentType);
 Assert.Equal(200, context.Response.StatusCode);
}
```

Как видите, промежуточное ПО для модульного тестирования требует большой настройки, чтобы заставить его работать, но есть и положительная сторона: это позволяет тестировать промежуточное ПО изолированно, но в  некоторых случаях, особенно для простых компонентов
без каких-либо зависимостей от баз данных или других служб, интеграционное тестирование может быть (что несколько удивительно) проще.

## Тестирование контроллеров и страницы Razoir Pages

```Csharp
public class CurrencyControllerTest
{
 [Fact]
 public void Convert_ReturnsValue()
 {
 var controller = new CurrencyController();
 var model = new ConvertInputModel
 {
 Value = 1,
 ExchangeRate = 3,
 DecimalPlaces = 2,
 };
 ActionResult<decimal> result = controller.Convert(model);
 Assert.NotNull(result);
 }
}
```

Тут важно отметить, что вы тестируете только возвращаемое значение действия, ActionResult<T>, а не ответ, который отправляется обратно
пользователю. Процесс сериализации результата в ответ обрабатывается инфраструктурой форматера MVC, как было показано в главе 9, а не
контроллером.


Контроллеры и  Razor Pages сами по себе не должны содержать бизнес-логику; они должны обращаться к другим сервисам. Рассматривайте
их скорее как оркестраторы, выступающие в качестве посредника между
HTTP-интерфейсами, которые предоставляет ваше приложение, и службами бизнес-логики.

**Замечание**: рассмотрет возможность применения MediatR

Razor Page
https://learn.microsoft.com/ru-ru/aspnet/core/test/razor-pages-tests?view=aspnetcore-8.0

Лично я  стараюсь избегать модульного тестирования контроллеров
API таким образом1
. Как уже было показано на примере привязки модели, контроллеры в некоторой степени зависят от более ранних этапов
фреймворка MVC, которые вам часто нужно эмулировать. Точно так же,
если ваши контроллеры обращаются к  HttpContext (доступен в  базовых классах ControllerBase), может потребоваться дополнительная настройка.

Вместо использования модульного тестирования я стараюсь сделать
так, чтобы мои контроллеры и  Razor Pages были как можно легковеснее. Я помещаю как можно больше поведения в этих классах в сервисы
бизнес-логики, для которых можно легко выполнить модульное тестирование, или в промежуточное ПО и фильтры, которые легче тестировать
независимо.


# Интеграционные тесты


```Csharp
public class StatusMiddlewareTests
{
 [Fact]
 public async Task StatusMiddlewareReturnsPong()
 {
 var hostBuilder = new HostBuilder()
 .ConfigureWebHost(webHost =>
 {
 webHost.Configure(app =>
 app.UseMiddleware<StatusMiddleware>());
 webHost.UseTestServer();
 });
 IHost host = await hostBuilder.StartAsync();
 HttpClient client = host.GetTestClient();
 var response = await client.GetAsync("/ping");
 response.EnsureSuccessStatusCode();
 var content = await response.Content.ReadAsStringAsync();
 Assert.Equal("pong", content);
 }
}
```

Если вы хотите запускать интеграционные тесты на основе существующего приложения, вам не нужно настраивать тестовый HostBuilder вручную. Вместо этого можно использовать еще один вспомогательный пакет, ```Microsoft.AspNetCore.Mvc.Testing```.

## Тестирование приложения с помощью класса WebApplicationFactory

```Csharp
 public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
 {
     private readonly WebApplicationFactory<Program> _fixture;
     public IntegrationTests(
     WebApplicationFactory<Program> fixture)
     {
         _fixture = fixture;
     }
     [Fact]
     public async Task PingRequest_ReturnsPong()
     {
         HttpClient client = _fixture.CreateClient();
         var response = await client.GetAsync("/ping");
         response.EnsureSuccessStatusCode();
         var content = await response.Content.ReadAsStringAsync();
         Assert.Equal("pong", content);
     }
 }
```

## Замена зависимостей в классе WebApplicationFactory

```Csharp
public class IntegrationTests:
 IClassFixture<WebApplicationFactory<Startup>>
{
 private readonly WebApplicationFactory<Startup> _fixture;
 public IntegrationTests(WebApplicationFactory<Startup> fixture)
 {
 _fixture = fixture;
 }

[Fact]
 public async Task ConvertReturnsExpectedValue()
 {
 var customFactory = _fixture.WithWebHostBuilder(
 (IWebHostBuilder hostBuilder) =>
 {
 hostBuilder.ConfigureTestServices(services =>
 {
 services.RemoveAll<ICurrencyConverter>();
 services.AddSingleton
 <ICurrencyConverter, StubCurrencyConverter>();
 });
 });
 HttpClient client = customFactory.CreateClient();
 var response = await client.GetAsync("/api/currency");
 response.EnsureSuccessStatusCode();
 var content = await response.Content.ReadAsStringAsync();
 Assert.Equal("3", content);
 }
}

```

### Уменьшение дублирования кода за счет создания специального класса WebApplicationFactory

```Csharp
public class CustomWebApplicationFactory
 : WebApplicationFactory<Startup>
{
 protected override void ConfigureWebHost(
 IWebHostBuilder builder)
 {
 builder.ConfigureTestServices(services =>
 {
 services.RemoveAll<ICurrencyConverter>();
 services.AddSingleton
 <ICurrencyConverter, StubCurrencyConverter>();
 });
 }
}
```

```Csharp
public class IntegrationTests:
 IClassFixture<CustomWebApplicationFactory>
{
 private readonly CustomWebApplicationFactory _fixture;
 public IntegrationTests(CustomWebApplicationFactory fixture)
 {
 _fixture = fixture;
 }
 [Fact]
 public async Task ConvertReturnsExpectedValue()
 {
 HttpClient client = _fixture.CreateClient();
 var response = await client.GetAsync("/api/currency");
 response.EnsureSuccessStatusCode();
 var content = await response.Content.ReadAsStringAsync();
 Assert.Equal("3", content);
 }
}
```

# Изоляция базы данных с помощью поставщика EF Core в памяти

Поставщики баз данных в памяти – это альтернативные поставщики,
предназначенные только для тестирования. Microsoft включает в  ASP.
NET Core двух поставщиков в памяти:

- Microsoft.EntityFrameworkCore.InMemory – этот поставщик не моделирует базу данных. Вместо этого он хранит объекты непосредственно в памяти. Это не реляционная база данных как таковая, поэтому она
не обладает всеми функциями обычной базы данных. Вы не можете
выполнить к ней SQL-запрос напрямую, и она не будет обеспечивать соблюдение ограничений, но работает она быстро;
- Microsoft.EntityFrameworkCore.Sqlite – SQLite – это реляционная база
данных. Она очень ограничена в функциях по сравнению с такой базой данных, как SQL Server, но это настоящая реляционная база данных, в отличие от поставщика базы данных в памяти. Обычно база
данных SQLite пишется в файл, но у поставщика есть режим in-memory, при котором база данных остается в  памяти. Это значительно
ускоряет и упрощает его создание и использование для тестирования.

```Csharp
[Fact]
public void GetRecipeDetails_CanLoadFromContext()
{
 var connection = new SqliteConnection("DataSource=:memory:");
 connection.Open();
 var options = new DbContextOptionsBuilder<AppDbContext>()
 .UseSqlite(connection)
 .Options;
 using (var context = new AppDbContext(options))
 {
 context.Database.EnsureCreated();
 context.Recipes.AddRange(
 new Recipe { RecipeId = 1, Name = "Recipe1" },
 new Recipe { RecipeId = 2, Name = "Recipe2" },
 new Recipe { RecipeId = 3, Name = "Recipe3" });
 context.SaveChanges();
 }
 using (var context = new AppDbContext(options))
 {
 var service = new RecipeService(context);
 var recipe = service.GetRecipe (id: 2);
 Assert.NotNull(recipe);
 Assert.Equal(2, recipe.Id);
 Assert.Equal("Recipe2", recipe.Name);
 }
}
```
Можно объединить этот код с CustomWebApplicationFactory, чтобы использовать базу данных в  памяти для своих
интеграционных тестов.
