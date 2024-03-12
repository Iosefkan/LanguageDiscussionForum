<script lang="ts">
  import { Router, Route } from 'svelte-routing';
  import Home from './pages/Home.svelte';
  import Login from './pages/Login.svelte';
  import Register from './pages/Register.svelte';
  import AuthorizeView from './AuthorizeView.svelte';
  import GetThings from './pages/GetThings.svelte';
</script>


  <Router>
    <Route path="/login" component={Login}/>
    <Route path="/register" component={Register}/>
    <Route component={GetThings}/>
  </Router>


