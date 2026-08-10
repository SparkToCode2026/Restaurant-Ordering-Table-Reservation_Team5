// CHANGE THIS to your ASP.NET Core API URL.
const API_BASE_URL="http://localhost:5073";
const API={
 tokenKey:"team5_token",userKey:"team5_user",
 get token(){return localStorage.getItem(this.tokenKey)},
 get user(){try{return JSON.parse(localStorage.getItem(this.userKey)||"null")}catch{return null}},
 setSession(t,u){localStorage.setItem(this.tokenKey,t);localStorage.setItem(this.userKey,JSON.stringify(u||{}))},
 clearSession(){localStorage.removeItem(this.tokenKey);localStorage.removeItem(this.userKey)},
 async request(path,opt={}){
  const headers={"Content-Type":"application/json",...(opt.headers||{})};
  if(this.token)headers.Authorization=`Bearer ${this.token}`;
  const r=await fetch(API_BASE_URL+path,{...opt,headers}),txt=await r.text();
  let d=null;try{d=txt?JSON.parse(txt):null}catch{d=txt}
  if(!r.ok)throw new Error(`${r.status}: ${typeof d==="string"?d:(d?.message||d?.title||JSON.stringify(d))}`);
  return d;
 },
 get(p){return this.request(p)},post(p,b){return this.request(p,{method:"POST",body:JSON.stringify(b)})},
 put(p,b){return this.request(p,{method:"PUT",body:JSON.stringify(b)})},patch(p,b){return this.request(p,{method:"PATCH",body:JSON.stringify(b)})},
 delete(p){return this.request(p,{method:"DELETE"})}
};