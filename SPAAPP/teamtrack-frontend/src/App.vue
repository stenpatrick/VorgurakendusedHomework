<template>
    <template v-if="shouldShowNavbar">
      <Navbar />
    </template>
    <router-view />
</template>

<script setup lang="ts">
import Navbar from "./components/Navbar.vue";
import { useRoute } from "vue-router";
import { defineProps, computed } from "vue";
const { isAthlete, hasChosen } = defineProps<{
  isAthlete: Boolean;
  hasChosen: Boolean;
}>();

const route = useRoute();

const shouldShowNavbar = computed(() => {
  // Check if the current route is not the homepage
  return route.name !== "Avaleht";
});
</script>

<style>
ul {
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  background-color: rgb(17 24 39);
  color: white
}

li {
  float: left;
  border-right:1px solid #bbb;
}

li:last-child {
  border-right: none;
}

li a {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}

li a:hover:not(.active) {
  background-color: #E935C1;
}

.active {
  background-color: #04AA6D;
}
</style>


