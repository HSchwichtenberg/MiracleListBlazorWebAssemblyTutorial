cd H:\git\Github\HeiseBlazorTutorial


#git cherry-pick --abort

git checkout Master
git cherry-pick fc2e9909d26f28f6176ab3f93442d4fe96a28d67
git commit -m "Update Jahreszahl"
git push