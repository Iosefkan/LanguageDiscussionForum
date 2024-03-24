<script lang="ts">
    import { Router, Route } from 'svelte-routing';
    import JwtRegister from './pages/JwtRegister.svelte';
    import Main from './pages/Main.svelte';
    import JwtLogin from './pages/JwtLogin.svelte';
    import Logout from './Logout.svelte';
    import QuestionPage from './pages/QuestionPage.svelte';
    import LanguageQuestions from './pages/LanguageQuestions.svelte';
    import Profile from './pages/Profile.svelte';
</script>


  <Router>
    <Route path="/login" component={JwtLogin}/>
    <Route path="/register" component={JwtRegister}/>
    <Route path="/logout" component={Logout}/>
    <Route path="/selectedQuestion/:id" let:params>
      <QuestionPage id="{Number.parseInt(params.id)}" />
    </Route>
    <Route path="/lang/:language" let:params>
      <LanguageQuestions language="{params.language}" />
    </Route>
    <Route path="/profile" component={Profile}/>
    <Route component={Main}/>
  </Router>


