import Vue from 'vue';
import App from './App.vue';
import VueRouter from 'vue-router';
import routes from './route.config';
import ElementUI from 'element-ui';
import resource from 'vue-resource';
import 'element-ui/lib/theme-default/index.css';
//import vueUrl from './vueUrl';

Vue.use(ElementUI);
Vue.use(VueRouter);
Vue.use(resource);

//const baseUrl="http://localhost:63428/api/";
//Vue.use(vueUrl,baseUrl);
const router = new VueRouter({
    mode: 'hash',
    base: __dirname,
    routes
});

document.title = 'WorkSpace';
Vue.http.options.root = "/api/";
var _app=new Vue({
  render: h => h(App),
  router
}).$mount('#app');