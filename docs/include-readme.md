---
title: Home
permalink: /
nav_order: 0
---

{% capture readme %}{% include_relative README.md %}{% endcapture %}
{{ readme | replace: ".md", ".html" | replace: "general/", "/" | markdownify }}
