import navConfig from './nav.config.json';
import langs from './i18n/route.json';

const registerRoute = (navConfig) => {
  let route = [];
  
  Object.keys(navConfig).forEach((lang, index) => {

    let navs = navConfig[lang];
    
    navs.forEach(group => {
      let groupRoute={
        path:group.path,
        children: [],
        title:group.groupName
      };

      if (group.list) {
        group.list.forEach(page => {
          const component =require(`./docs/${ lang }${page.path}.vue`);
          
            route.push({path: '/'+lang+page.path,component:component});
        });
      }
    });


  });


  route.push({path: '/',redirect:'/zh-CN/home'});
  return route;
};

let route = registerRoute(navConfig);

export default route;
